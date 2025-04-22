using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System;
using System.Diagnostics;
using System.IO;


namespace CIMEX_Project;

public partial class VisitWindow : Window
{
    public ObservableCollection<string> Tasks { get; set; }
    private ObservableCollection<TeamMember> TeamMembersListNewVisit { get; set; }
    public DateTime VisitStartDate { get; set; }
    public DateTime VisitEndDate { get; set; }
    public string Titel { get; set; }
    private  bool _actualVisit;
    private readonly Patient _patient;
    private PatientsVisit _patientsVisit;
    private List<StructureOfVisit> _visitsStructure;
    private Investigator _investigator;



    public VisitWindow(PatientsVisit patientsVisit, Patient patient)
    {
        _patient = patient;
        _patientsVisit = patientsVisit;
        Console.WriteLine($"Visit window initialization {_patient.StudyName}  {_patientsVisit.Name}");

        InitializeComponent();
        InitializePatientWindowUi();
    }

    private async void InitializePatientWindowUi()
    {
        Tasks = new ObservableCollection<string>();
        await GetTasks(); // обязательно await
        if (_patient.NextPatientsVisit.Name.Equals(_patientsVisit.Name))
        {
            _actualVisit = true;
        }
        else
        {
            _actualVisit = false;
        }

        if (!_actualVisit)
        {
            PrintVisitButton.Visibility = Visibility.Collapsed;
            SendReminderButton.Visibility = Visibility.Collapsed;
            ConfirmVisitButton.Visibility = Visibility.Collapsed;
        }
        _investigator = await GetInvestigator();
         _patientsVisit.AssignedInvestigator = _investigator;// await и корректное название метода

         Titel = $"Patient: {_patient.Surname} {_patient.Name}\n" +
                 $"Study: {_patient.StudyName}\n" +
                 $"Visit: {_patientsVisit.Name} {_patientsVisit.DateOfVisit:dd.MM.yyyy}\n";
         if (_actualVisit)
         {
            Titel = Titel +  $"Assigned Investigator: {_investigator.Name} {_investigator.Surname}";
         }

         DataContext = this;
    }

    private async Task GetTasks()
    {
        DaoVisitMongoDb daoVisitMongoDb = new DaoVisitMongoDb();
        _visitsStructure = await daoVisitMongoDb.GetStructureOfAllVisits(_patient.StudyName);

        var visit = _visitsStructure.FirstOrDefault(x => x.Name == _patientsVisit.Name);
        if (visit != null)
        {
            Console.WriteLine($"Visit found: {visit.Name}");
            foreach (var task in visit.Tasks)
            {
                Tasks.Add(task);
            }
        }
        else
        {
            Console.WriteLine("Visit not found in structure.");
        }

        _patientsVisit.Tasks = Tasks.ToList();
    }

    private async Task<Investigator> GetInvestigator()
    {
        DAOTeamMemeberNeo4j daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();
        var teamMember = await daoTeamMemeberNeo4J.GetAttendingTeamMember(_patient.PatientHospitalId);

        // Предполагаем, что teamMember уже типа Investigator или конвертируем
        return teamMember as Investigator ?? new Investigator
        {
            Name = teamMember.Name,
            Surname = teamMember.Surname,
            Email = teamMember.Email,
            Role = teamMember.Role
        };
    }
    
    private async Task CreateListOfTeamInvestigators()
    {
        Console.WriteLine("CreateListOfTeamMembers started");
        DAOTeamMemeberNeo4j daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();
        var fullTeam = await daoTeamMemeberNeo4J.GetAllStudyTeamMembers(_patient.StudyName);
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

    private async void PrintVisit(object sender, RoutedEventArgs e)
    {
        PdfHandler pdfHandler = new PdfHandler();
        await pdfHandler.PrintVisit(_patientsVisit, _patient);
    }

    private async void SendReminder(object sender, RoutedEventArgs e)
    {
        var vistStructure =  _visitsStructure.FirstOrDefault(v => v.Name == _patientsVisit.Name);
        await _investigator.SendReminder(_investigator, _patient, vistStructure);
    }

    private async void ConfirmVisit(object sender, RoutedEventArgs e)
    {
        ButtonGrid.Visibility = Visibility.Collapsed;
        TitelBlock.Visibility = Visibility.Collapsed;
        TaskList.Visibility = Visibility.Collapsed;
        TeamMember.Visibility = Visibility.Visible;
        NewVisitTitelBlock.Visibility = Visibility.Visible;
        var nextVisitData = await _patientsVisit.CreateVisitData(_patient, _patientsVisit, _visitsStructure);
        VisitStartDate = nextVisitData.StartDate;
        VisitEndDate = nextVisitData.EndDate;
        _patientsVisit = nextVisitData.NewPatientVisit;

// Обновить DataContext, чтобы изменения были видны для биндинга
        DataContext = null;
        DataContext = this;
        await CreateListOfTeamInvestigators();
    }

    private void AddButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is Investigator investigator)
        {
            AssignedInvestigator.Text = $"Assigned Dr.{investigator.Surname} {investigator.Name}";
            _investigator = investigator;
        }
    }

    private async void AssignVisit(object sender, RoutedEventArgs e)
    {

        _patientsVisit.AssignedInvestigator = _investigator;
        _patientsVisit.DateOfVisit = (DateTime)VisitPicker.SelectedDate;
        await _patient.CreateNewVisit(_patient, _patientsVisit);
        this.Close();
    }
}