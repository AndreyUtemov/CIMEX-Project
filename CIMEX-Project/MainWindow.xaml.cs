using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CIMEX_Project;

public partial class MainWindow : Window
{
    private static MainWindowManagement _mainWindowManagement = new MainWindowManagement();

    public MainWindow()
    {
        InitializeComponent();
        InitializeUi(); // Асинхронная инициализация
    }

    private async void InitializeUi()
    {
        try
        {
            DataContext = new MainViewModel
            {
                Date = DateTime.Now.ToString("dd MMMM yyyy"),
                LeftTitle = _mainWindowManagement.GetLeftTitle(),
                RightTitle = _mainWindowManagement.GetRightTitle()
            };

            var allButtons = _mainWindowManagement.GetMainButtons();

            await AddUpperButtons(allButtons.Studies);
            AddMiddleButtons(allButtons.Screened);
            AddBottomButtons(allButtons.Included);
        }
        catch (Exception e)
        {
            // TODO handle exception
        }
    }

    public class MainViewModel
    {
        public string Date { get; set; }
        public string LeftTitle { get; set; }
        public string RightTitle { get; set; }
    }

    private async Task AddUpperButtons(List<Button> upperButtons)
    {
        UpperPanel.Children.Clear();

        foreach (Button button in upperButtons)
        {
            button.Click -= Study_Button_Click;
            button.Click += Study_Button_Click;
            UpperPanel.Children.Add(button);
        }
    }

    private void AddMiddleButtons(List<Button> middleButtons)
    {
        MiddlePanel.Children.Clear();

        foreach (Button button in middleButtons)
        {
            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01B0FF"));
            MiddlePanel.Children.Add(button);
            button.Click += (sender, e) => Patient_Button_Click(sender, e);
        }
    }

    private void AddBottomButtons(List<Button> bottomButtons)
    {
        BottomPanel.Children.Clear();

        foreach (Button button in bottomButtons)
        {
            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0085D4"));
            BottomPanel.Children.Add(button);
            button.Click += (sender, e) => Patient_Button_Click(sender, e);
        }
    }

    private void Study_Button_Click(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        
        Study study = (Study)button.Tag;
        _mainWindowManagement.SetStudyWindow(study);
       InitializeUi();

        // TODO study button logic: change buttons , create user with new class 
    }

    private async void Patient_Button_Click(object sender, RoutedEventArgs e)
    {
       Button button = sender as Button;
       Patient patient = (Patient)button.Tag;
       TeamMember user = await _mainWindowManagement.GetUser(patient);
       PatientWindowManagement patientWindowManagement = new PatientWindowManagement();
       patientWindowManagement.SetPatientWindow(patient, user);
       PatientWindow patientWindow = new PatientWindow();
       patientWindow.Show();
       this.Hide();

    }
}