using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CIMEX_Project;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static MainWindowManagement _mainWindowManagement = new MainWindowManagement();

    public MainWindow()
    {
        InitializeComponent();
        LoadDataAndInitializeUI(); // Асинхронная инициализация
    }

    private async void LoadDataAndInitializeUI()
    {
        try
        {
            await _mainWindowManagement.ProgrammStart("admin@cimex.at"); // Загружаем данные

            DataContext = new MainViewModel
            {
                Date = DateTime.Now.ToString("dd MMMM yyyy"),
                LeftTitle = _mainWindowManagement.GetLeftTitle(),
                RightTitle = _mainWindowManagement.GetRightTitle()
            };
           await AddUpperButtons(_mainWindowManagement.CreateUpperRowButtons());
            AddMiddleButtons(_mainWindowManagement.CreateMiddleRowButtons());
            AddBottomButtons(_mainWindowManagement.CreateBottomRowButtons());
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

    private async Task AddUpperButtons(Task<List<Button>> upperButtons) // Асинхронный метод
    {
        UpperPanel.Children.Clear();
        List<Button> buttons = await upperButtons; // Дожидаемся результата
        foreach (Button button in buttons) // Теперь итерируемся по List<Button>
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
        
        // TODO study button logic: change buttons , create user with new class 
    }

    private void Patient_Button_Click(object sender, RoutedEventArgs e)
    {
        // TODO patient button logic : create patient window 
    }
}