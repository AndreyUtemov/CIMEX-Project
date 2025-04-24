using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CIMEX_Project;

public partial class NewPatientWindow : Window
{
    public string StudyTitle { get; set; }
    private TeamMember _teamMember;
    private Study _study;
    private Investigator _investigator;
    private ObservableCollection<TeamMember> TeamMembersListNewVisit { get; set; }

    public NewPatientWindow(Study study, TeamMember user)
    {
        _teamMember = user;
        _study = study;
        InitializeComponent();
        
        StudyTitle = $"Enter new patient in study {study.StudyName}";
        DataContext = this;
        Loaded += async (s, e) => await CreateListOfTeamInvestigators();
    }

    private async void AddPatient(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CIDBox.Text) ||
            string.IsNullOrWhiteSpace(SurnameBox.Text) ||
            string.IsNullOrWhiteSpace(FirstnameBox.Text) ||
            !ScreeningPicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Enter all data", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
            string patientId = CIDBox.Text;
            string name = FirstnameBox.Text;
            string surname = SurnameBox.Text;
            DateTime screeningDate = ScreeningPicker.SelectedDate.Value;
            PatientsVisit patientsVisit = new PatientsVisit("Screening", screeningDate, 0, true);
            Patient patient = new Patient
            {
                Name = name,
                Surname = surname,
                PatientHospitalId = patientId,
                StudyName = _study.StudyName,
                NextPatientsVisit = patientsVisit
            };
            await ShowMessageBox(patient);
        }
    }

    private async Task ShowMessageBox(Patient patient)
    {
        string title = $"Add patient to {patient.StudyName}?";
        string message = $"ClinicalID {patient.PatientHospitalId}\nSurname {patient.Surname}\nName {patient.Name}" +
                         $"\nScreening on {patient.NextPatientsVisit.DateOfVisit:dd.MM.yyyy}";
        var result = MessageBox.Show(message, title, MessageBoxButton.OKCancel);
        if (result == MessageBoxResult.OK)
        {
            await _teamMember.AddNewPatient(patient, _investigator);
            MessageBox.Show("Patient added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

         this.DialogResult = true;
        }
        else if(result == MessageBoxResult.Cancel)
        {
            CIDBox.Clear();
            FirstnameBox.Clear();
            SurnameBox.Clear();
            ScreeningPicker.SelectedDate = null;
        }
    }

    private void AddButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is Investigator investigator)
        {
            AssignedInvestigator.Text = $"Assigned Dr.{investigator.Surname} {investigator.Name}";
            _investigator = investigator;
        }
    }
    
    private async Task CreateListOfTeamInvestigators()
    {
        Console.WriteLine("CreateListOfTeamMembers started");
        DAOTeamMemeberNeo4j daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();
        var fullTeam = await daoTeamMemeberNeo4J.GetAllStudyTeamMembers(_study.StudyName);
        List<TeamMember> team = new List<TeamMember>();
        foreach (var teamMemeber in fullTeam)
        {
            Console.WriteLine($"{teamMemeber.Surname}  {teamMemeber.Role}");
            if (teamMemeber.Role == "Study Investigator" || teamMemeber.Role == "Medical Doctor")
            {
                TeamMemberFactory teamMemberFactory = new TeamMemberFactory();
                Investigator investigator = (Investigator)teamMemberFactory.CreateUserForStudy("Investigator", teamMemeber);
                team.Add(investigator);
            }
        }
        
        TeamMembersListNewVisit = new ObservableCollection<TeamMember>(team);
        TeamMembersList.ItemsSource = TeamMembersListNewVisit;

    }
}