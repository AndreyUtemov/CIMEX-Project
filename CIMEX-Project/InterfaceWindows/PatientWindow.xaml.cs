using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CIMEX_Project;

public partial class PatientWindow : Window
{
    private Patient _patient;
    private TeamMember _user;
    List<StructureOfVisit> _visitsStructure;
    private List<PatientsVisit> _visitList; 

    public PatientWindow(Patient patient, TeamMember user)
    {
        _user = user;
        _patient = patient;
        InitializeComponent();
        InitializePatientWindowUi();
    }

    private async void InitializePatientWindowUi()
    {
        try
        {
            var allVisitButtons = await GetVisitButtons(_patient);
          await AddButtons(allVisitButtons);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Fail in InitializePatientWindowUi: {e.Message}");
        }

        DataContext = new PatientViewModel
        {
            Titel = $"{_patient.Surname} {_patient.Name}\n{_patient.StudyName}"
        };
    }

    public class PatientViewModel
    {
        public string Titel { get; set; }
    }

    private async Task AddButtons(List<Button> buttonList)
    {
        VisitButtonPanel.Children.Clear();

        foreach (Button button in buttonList)
        {
            Console.WriteLine($"Button {button.Content} placing");
            button.Click -= VisitButtonClick;
            button.Click += VisitButtonClick;
            VisitButtonPanel.Children.Add(button);
        }
    }

    private void VisitButtonClick(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        PatientsVisit patientsVisit = (PatientsVisit)button.Tag;
        VisitWindow visitWindow = new VisitWindow(patientsVisit, _patient);
        visitWindow.Owner = this;
        bool? result = visitWindow.ShowDialog();
        if (result == true)
        {
            Console.WriteLine($"Returned visit {visitWindow.Result.Name}");
            _patient.NextPatientsVisit = visitWindow.Result;
            InitializePatientWindowUi();
        }
        
    }

    public async Task<List<Button>> GetVisitButtons(Patient patient)
    {
        DaoVisitMongoDb daoVisitMongoDb = new DaoVisitMongoDb();
        List<PatientsVisit> _visitList = await daoVisitMongoDb.GetPatientVisits(patient.PatientHospitalId);
        foreach (var visit in _visitList)
        {
            Console.WriteLine($"Next visit {_patient.NextPatientsVisit.Name}");
            if (visit.Name == patient.NextPatientsVisit.Name)
            {
                visit.IsScheduled = true;
                visit.DateOfVisit = patient.NextPatientsVisit.DateOfVisit;
            }
            else
            {
                visit.IsScheduled = false;
            }
        }

        ButtonFactory buttonFactory = new ButtonFactory();
        List<Button> buttonList = await buttonFactory.CreateVisitButtons(_visitList);
        return buttonList;
    }
}