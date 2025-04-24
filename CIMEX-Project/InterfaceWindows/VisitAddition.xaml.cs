using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CIMEX_Project;

public partial class VisitAddition : Window
{
    public ObservableCollection<string> Tasks { get; set; }

    public StructureOfVisit Result { get; set; }
    private int _visitNumber;

    public VisitAddition(int visitNumber)
    {
        _visitNumber = visitNumber;
        InitializeComponent();
        InitializingUI();
        Tasks = new ObservableCollection<string>();
        TaskList.ItemsSource = Tasks;
        DataContext = this;
    }

    private void InitializingUI()
    {
        switch (_visitNumber)
        {
            case 0:
                TitleBox.Text = "Screening";
                TimeWindowLabel.Visibility = Visibility.Collapsed;
                TaskBox.Text = "Sign informed consent";
                DayAfterRandomization.Content = "Interval before randomization(days)";
                TimeWindowBox.Text = "0";
                TimeWindowBox.Visibility = Visibility.Collapsed;
                break;

            case 1:
                TitleBox.Text = "Randomization";
                DayAfterRandomization.Visibility = Visibility.Collapsed;
                TaskBox.Text = "Randomize Patient";
                TimeWindowBox.Text = "0";
                TimeWindowBox.Visibility = Visibility.Collapsed;
                TimeWindowLabel.Visibility = Visibility.Collapsed;
                IntervalBox.Text = "0";
                IntervalBox.Visibility = Visibility.Collapsed;
                break;
        }
    }

    private void AddVisit(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleBox.Text) || string.IsNullOrWhiteSpace(IntervalBox.Text) ||
            string.IsNullOrWhiteSpace(TimeWindowBox.Text) || Tasks.ToList().Count == 0
           )
        {
            MessageBox.Show("Enter all data", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
            StructureOfVisit structureOfVisit = new StructureOfVisit
            {
                Name = TitleBox.Text,
                TimeWindow = int.Parse(TimeWindowBox.Text),
                PeriodAfterRandomization = int.Parse(IntervalBox.Text),
                Tasks = Tasks.ToList()
            };
            if (_visitNumber == 0)
            {
                structureOfVisit.PeriodAfterRandomization *= -1;
            }
            Result = structureOfVisit;
            this.DialogResult = true;
            this.Close();
        }
    }

    private void AddVTask(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TaskBox.Text))
        {
            MessageBox.Show("Enter Task", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
            Tasks.Add(TaskBox.Text);
            TaskBox.Text = string.Empty;
            
        }
    }

    private void SetOnlyNumbers(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !e.Text.All(char.IsDigit);
    }
}