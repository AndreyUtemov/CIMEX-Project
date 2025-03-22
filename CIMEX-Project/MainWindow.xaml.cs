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
    private static CenterManagement _centerManagement = new CenterManagement();

    public MainWindow()
    {
        InitializeComponent();
        LoadDataAndInitializeUI(); // Асинхронная инициализация
    }

    private async void LoadDataAndInitializeUI()
    {
        try
        {
            await _centerManagement.ProgrammStart("admin@cimex.at"); // Загружаем данные

            DataContext = new MainViewModel
            {
                Date = DateTime.Now.ToString("dd MMMM yyyy"),
                LeftTitle = _centerManagement.GetLeftTitle(),
                RightTitle = _centerManagement.GetRightTitle()
            };
           await AddUpperButtons(_centerManagement.CreateUpperRowButtons());
            AddMiddleButtons(_centerManagement.CreateMiddleRowButtons());
            AddBottomButtons(_centerManagement.CreateBottomRowButtons());
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
            button.Click -= Button_Click;
            button.Click += Button_Click;
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
            button.Click += (sender, e) => Button_Click(sender, e);
        }
    }

    private void AddBottomButtons(List<Button> bottomButtons)
    {
        BottomPanel.Children.Clear();

        foreach (Button button in bottomButtons)
        {
            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0085D4"));
            BottomPanel.Children.Add(button);
            button.Click += (sender, e) => Button_Click(sender, e);
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        // Логика обработки клика по кнопке
    }
}