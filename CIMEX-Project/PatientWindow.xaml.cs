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
            Console.WriteLine("try to get visit buttons");
            var allVisitButtons = await GetVisitButtons(_patient);
            Console.WriteLine("We have visit buttons and try to test it");
            foreach (var button in allVisitButtons)
            {
                Console.WriteLine($"Button {button.Content} is here");
            }

            Console.WriteLine("We try to place a buttons");
            await AddButtons(allVisitButtons);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка в InitializePatientWindowUi: {e.Message}");
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
        visitWindow.ShowDialog();
    }

    public async Task<List<Button>> GetVisitButtons(Patient patient)
    {
        DaoVisitMongoDb daoVisitMongoDb = new DaoVisitMongoDb();
        Console.WriteLine("Creating of visit buttons list in GetVisitButtons  PatientWindowManagement");
     
        List<PatientsVisit> _visitList = await daoVisitMongoDb.GetPatientVisits(patient.PatientHospitalId);
        foreach (var visit in _visitList)
        {
            if (visit.Name == patient.NextPatientsVisit.Name)
            {
                visit.IsScheduled = true;
            }
            else
            {
                visit.IsScheduled = false;
            }

            Console.WriteLine($"Visit - {visit.Name}  Date - {visit.DateOfVisit}");
        }

        ButtonFactory buttonFactory = new ButtonFactory();
        List<Button> buttonList = await buttonFactory.CreateVisitButtons(_visitList);
        foreach (var button in buttonList)
        {
            Console.WriteLine(button.Tag.ToString());
        }

        return buttonList;
    }
}