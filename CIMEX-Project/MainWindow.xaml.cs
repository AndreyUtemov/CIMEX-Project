using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


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
        DataContext = new MainViewModel
        {
            Date = DateTime.Now.ToString("dd MMMM yyyy"),
            LeftTitle = _centerManagement.GetLeftTitle(),
            RightTitle = _centerManagement.GetRightTitle()
        };

        AddUpperButtons(_centerManagement.CreateUpperRowButtons());
        AddMiddleButtons(_centerManagement.CreateMiddleRowButtons());
        AddBottomButtons(_centerManagement.CreateBottomRowButtons());
    }

    public class MainViewModel
    {
        public string Date { get; set; }
        public string LeftTitle { get; set; }
        public string RightTitle { get; set; }
    }

    private void AddUpperButtons(List<Button> upperButtons)
    {
        int column = 1;

        UpperPanel.Children.Clear();

        foreach (Button button in upperButtons)
        {
            button.Click += (sender, e) => Button_Click(sender, e);
            UpperPanel.Children.Add(button);
        }
    }

    private void AddMiddleButtons(List<Button> middleButtons)
    {
        int column = 1;
        int row = 2;

        BottomPanel.Children.Clear();

        foreach (Button button in middleButtons)
        {
            BottomPanel.Children.Add(button);
            button.Click += (sender, e) => Button_Click(sender, e);
        }
    }

    private void AddBottomButtons(List<Button> bottomButtons)
    {
        int column = 1;
        int row = 2;

        BottomPanel.Children.Clear();

        foreach (Button button in bottomButtons)
        {
            BottomPanel.Children.Add(button);
            button.Click += (sender, e) => Button_Click(sender, e);
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e, string trail)
    {
    }
}