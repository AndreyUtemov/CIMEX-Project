using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace CIMEX_Project;

public partial class MainWindow : Window
{
    private static AllProgrammManagement _allProgrammManagement = new AllProgrammManagement();
    private static List<Button> _studyButtons;
    private static List<Button> _screenedButtons;
    private static List<Button> _includedButtons;

    public MainWindow()
    {
        SetButtons();
        InitializeComponent();
        InitializeUi();
    }

    private async Task InitializeUi()
    {
        try
        {
            DataContext = new MainViewModel
            {
                Date = DateTime.Now.ToString("dd MMMM yyyy"),
                LeftTitle = _allProgrammManagement.GetLeftTitle(),
                RightTitle = _allProgrammManagement.GetRightTitle()
            };
        }
        catch (Exception e)
        {
            // TODO handle exception
        }

        AddUpperButtons(_studyButtons);
        AddMiddleButtons(_screenedButtons);
        AddBottomButtons(_includedButtons);
    }

    public class MainViewModel
    {
        public string Date { get; set; }
        public string LeftTitle { get; set; }
        public string RightTitle { get; set; }
    }

    private async Task SetButtons()
    {
        var buttonList = await _allProgrammManagement.ProgrammStart();
        _studyButtons = buttonList.Studies;
        _screenedButtons = buttonList.Included;
        _includedButtons = buttonList.Included;
    }

    private void AddUpperButtons(List<Button> upperButtons)
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
        var studyWindowData = _allProgrammManagement.SetStudyWindow(study);
        _includedButtons = studyWindowData.Included;
        _screenedButtons = studyWindowData.Screened;
        InitializeUi();
    }

    private void Patient_Button_Click(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        Patient patient = (Patient)button.Tag;
        TeamMember user = _allProgrammManagement.GetUserRoleForPatient(patient);
        PatientWindow patientWindow = new PatientWindow(patient, user);
        patientWindow.Closed += (s, args) => this.Show();
        patientWindow.Show();
        this.Hide();
    }

    public void AddPatientButtonClick(object sender, RoutedEventArgs routedEventArgs)
    {
        TeamMember user = _allProgrammManagement.GetUser();
        Study study = _allProgrammManagement.GetStudy();
        NewPatientWindow newPatientWindow = new NewPatientWindow(study, user);
        newPatientWindow.Owner = this; // Устанавливаем владельца
        newPatientWindow.ShowDialog(); // О
    }

    public void VisitStudyDocumentsPage(object sender, RoutedEventArgs routedEventArgs)
    {
        TeamMember user = _allProgrammManagement.GetUser();
        Study study = _allProgrammManagement.GetStudy();
        StudyDocuments studyDocuments = new StudyDocuments();
        studyDocuments.Owner = this; // Устанавливаем владельца
        studyDocuments.ShowDialog(); // О 
    }

    public void ChangeStudyTeam(object sender, RoutedEventArgs routedEventArgs)
    {
        TeamMember user = _allProgrammManagement.GetUser();
        Study study = _allProgrammManagement.GetStudy();
        StudyManagement studyManagement = new StudyManagement(user, study);
        studyManagement.Owner = this; // Устанавливаем владельца
        studyManagement.ShowDialog(); // О
    }
}