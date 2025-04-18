using System.Windows;

namespace CIMEX_Project;

public partial class NewPatientWindow : Window
{
    public string StudyTitle { get; set; }
    private TeamMember _teamMember;
    private Study _study;

    // Конструктор с передачей названия исследования
    public NewPatientWindow(Study study, TeamMember user)
    {
        _teamMember = user;
        _study = study;
        InitializeComponent();
        StudyTitle = $"Enter new patient in study {study.StudyName}";
        DataContext = this;
    }

    private void AddPatient(object sender, RoutedEventArgs e)
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
            PatientsVisit patientsVisit = new PatientsVisit("Screening", screeningDate, 0, "scheduled");
            Patient patient = new Patient(name, surname, patientId, _study.StudyName, "screening", patientsVisit);
            ShowMessageBox(patient);
        }
    }

    private void ShowMessageBox(Patient patient)
    {
        string title = $"Add patient to {_study.StudyName}?";
        string message = $"ClinicalID {patient.PatientHospitalId}\nSurname {patient.Surname}\nName {patient.Name}" +
                         $"\nScreening on {patient.NextPatientsVisit.DateOfVisit:dd.MM.yyyy}";
        var result = MessageBox.Show(message, title, MessageBoxButton.OKCancel);
        if (result == MessageBoxResult.OK)
        {
            _teamMember.AddNewPatient(patient);
            MessageBox.Show("Patient added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Закрытие окна
            this.Close();
        }
        else if(result == MessageBoxResult.Cancel)
        {
            CIDBox.Clear();
            FirstnameBox.Clear();
            SurnameBox.Clear();
            ScreeningPicker.SelectedDate = null;
        }
    }
}