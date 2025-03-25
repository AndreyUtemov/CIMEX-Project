using System.Windows;
using System.Windows.Controls;

namespace CIMEX_Project;

public partial class PatientWindow : Window
{
    private static PatientWindowManagement _patientWindowManagement = new PatientWindowManagement();

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
            button.Click -= VisitButtonClick;
            button.Click += VisitButtonClick;
            VisitButtonPanel.Children.Add(button);
        }
    }

    private void VisitButtonClick(object sender, RoutedEventArgs e)
    {
        // TODO visit button logic, create visit window 
    }
    
    
    
}