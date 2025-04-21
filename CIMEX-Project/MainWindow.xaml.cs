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
    private static MainWindowManagement _mainWindowManagement = new MainWindowManagement();
    private static List<Button> _studyButtons;
    private static List<Button> _screenedButtons;
    private static List<Button> _includedButtons;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded; // ждём полной загрузки окна
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await SetButtons();          // корректно дожидаемся кнопок
        await InitializeUi();        // и только потом инициализируем интерфейс
    }

    private async Task InitializeUi()
    {
        try
        {
            DataContext = new MainViewModel
            {
                Date = DateTime.Now.ToString("dd MMMM yyyy"),
                LeftTitle = _mainWindowManagement.GetLeftTitle(),
                RightTitle = _mainWindowManagement.GetRightTitle()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception in IntializeUI");
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
        Console.WriteLine("We setting buttonns");
        var buttonList = await _mainWindowManagement.ProgrammStart();
        _studyButtons = buttonList.Studies;
        Console.WriteLine("Study buttons ready");
        _screenedButtons = buttonList.Screened;
        Console.WriteLine("Screned buttons ready");
        _includedButtons = buttonList.Included;
        Console.WriteLine("Included butttons ready");
    }

    private void AddUpperButtons(List<Button> upperButtons)
    {
        UpperPanel.Children.Clear();
        Console.WriteLine("Creating buttons for ui");

        foreach (Button button in upperButtons)
        {
            Console.WriteLine($"Create button {button.Content.ToString()}");
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
        VisitDocumentButton.Visibility = Visibility.Visible;
        AddPatientButton.Visibility = Visibility.Visible;
        if (study.RoleOfUser == "Principal Investigator")
        {
            ChangeTeamButton.Visibility = Visibility.Visible;
        }
        else
        {
            ChangeTeamButton.Visibility = Visibility.Collapsed;
        }
        var studyWindowData = _mainWindowManagement.SetStudyWindow(study);
        _includedButtons = studyWindowData.Included;
        _screenedButtons = studyWindowData.Screened;
        InitializeUi();
    }

    private void Patient_Button_Click(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        Patient patient = (Patient)button.Tag;
        TeamMember user = _mainWindowManagement.GetUserRoleForPatient(patient);
        PatientWindow patientWindow = new PatientWindow(patient, user);
        patientWindow.Closed += (s, args) => this.Show();
        patientWindow.Show();
        this.Hide();
    }

    public void AddPatientButtonClick(object sender, RoutedEventArgs routedEventArgs)
    {
        TeamMember user = _mainWindowManagement.GetUser();
        Study study = _mainWindowManagement.GetStudy();
        NewPatientWindow newPatientWindow = new NewPatientWindow(study, user);
        newPatientWindow.Owner = this; // Устанавливаем владельца
        newPatientWindow.ShowDialog(); // О
    }

    public void VisitStudyDocumentsPage(object sender, RoutedEventArgs routedEventArgs)
    {
        TeamMember user = _mainWindowManagement.GetUser();
        Study study = _mainWindowManagement.GetStudy();
        StudyDocuments studyDocuments = new StudyDocuments();
        studyDocuments.Owner = this; // Устанавливаем владельца
        studyDocuments.ShowDialog(); // О 
    }

    public void ChangeStudyTeam(object sender, RoutedEventArgs routedEventArgs)
    {
        TeamMember user = _mainWindowManagement.GetUser();
        Study study = _mainWindowManagement.GetStudy();
        StudyManagement studyManagement = new StudyManagement(user, study);
        studyManagement.Owner = this; // Устанавливаем владельца
        studyManagement.ShowDialog(); // О
    }
}