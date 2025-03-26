using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CIMEX_Project;

public partial class PatientWindow : Window
{
    private static PatientWindowManagement _patientWindowManagement = new PatientWindowManagement();
    private static bool _visitscreen = false;

    public PatientWindow()
    {
        InitializeComponent();
        InitializePatientWindowUi();
    }

    public class PatientViewModel
    {
        public string PatientName { get; set; }
        public string StudyName { get; set; }
    }

    private async void InitializePatientWindowUi()
    {
        try
        {
            DataContext = new PatientViewModel()
            {
                PatientName = _patientWindowManagement.GetPatientName(),
                StudyName = _patientWindowManagement.GetStudyName(),
            };

            var allVisitButtons = _patientWindowManagement.GetVisitButtons();

            await AddButtons(allVisitButtons);
        }
        catch (Exception e)
        {
            // TODO handle exception
        }
    }

    private async Task AddButtons(List<Button> buttonList)
    {
        VisitButtonPanel.Children.Clear();

        foreach (Button button in buttonList)
        {
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

        _patientWindowManagement.SetVisitData(visit);
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
    
}