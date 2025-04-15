using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CIMEX_Project;

public partial class PatientWindow : Window
{
    private bool _visitscreen = false;
    private Patient _patient;
    private TeamMember _user;
   
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
            PatientWindowManagement _patientWindowManagement = new PatientWindowManagement();
            Console.WriteLine("try to get visit buttons");
            var allVisitButtons = await _patientWindowManagement.GetVisitButtons(_patient);
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
            
            if (!_visitscreen)
            {
            button.Click -= VisitButtonClick;
            button.Click += VisitButtonClick;
            }
            else
            {
                button.Click -= VisitButtonClick;
                button.Click += ManipulationButtonClick; 
            }

            VisitButtonPanel.Children.Add(button);
        }
    }

    private void VisitButtonClick(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;

        Visit visit = (Visit)button.Tag;

        // _patientWindowManagement.SetVisitData(visit);
        _visitscreen = true;
        // TODO visit button logic, create visit window 
    }

    private void ManipulationButtonClick(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var manipulation = button.Tag;
        button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0E239A"));
        button.Click -= ManipulationButtonClick;
    }

    private void CloseWindow_Click(object sender, RoutedEventArgs e)
    {
        this.Close(); // Закрываем окно (можно нажать крестик или кнопку)
    }
}