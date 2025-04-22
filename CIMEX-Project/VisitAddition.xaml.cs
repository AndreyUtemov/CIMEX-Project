using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CIMEX_Project;

public partial class VisitAddition : Window
{
    public ObservableCollection<string> Tasks { get; set; }
    
    public StructureOfVisit Result { get; set; }

    public VisitAddition(int visitNummer)
    {
        InitializeComponent();
       Tasks = new ObservableCollection<string>();
        TaskList.ItemsSource = Tasks;
        DataContext = this;
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
}