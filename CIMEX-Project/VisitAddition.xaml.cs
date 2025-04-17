using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CIMEX_Project;

public partial class VisitAddition : Window
{
    private List<string> _taskList = new List<string>();
    public ObservableCollection<string> Tasks { get; set; }
    public ObservableCollection<string> TimeIntervals { get; set; }

    public List<string> Result { get; set; }

    public VisitAddition()
    {
        InitializeComponent();
        TimeIntervals = new ObservableCollection<string>(Enum.GetNames(typeof(TimeInterval)));
        Tasks = new ObservableCollection<string>();
        TaskList.ItemsSource = Tasks;
    }

    private void AddVisit(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleBox.Text) || string.IsNullOrWhiteSpace(IntervalBox.Text) ||
            _taskList.Count == 0 || string.IsNullOrWhiteSpace(TimeWindowBox.Text)
            || !(UnitBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.IsEnabled))
        {
            MessageBox.Show("Enter all data", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
            Result = _taskList;
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
            _taskList.Add(TaskBox.Text);
            Tasks.Add(TaskBox.Text);
            TaskBox.Text = string.Empty;
        }
    }
}