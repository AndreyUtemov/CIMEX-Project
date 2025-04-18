using System.Collections.ObjectModel;
using System.Windows;

namespace CIMEX_Project;

public partial class AdminWindow : Window
{
    private ObservableCollection<Study> _studyCollection = new ObservableCollection<Study>();
    public AdminWindow()
    {
        InitializeComponent();
        CreateListOfStudies();
    }

    private async void CreateListOfStudies()
    {
        Console.WriteLine("CreateListOfStudies started");
        DAOAdminNeo4j daoAdminNeo4J = new DAOAdminNeo4j();
        var allStudies = await daoAdminNeo4J.GetAllStudy();
        foreach (var study in allStudies)
        {
            Console.WriteLine($"{study.StudyName} in List");
            
        }

        _studyCollection = new ObservableCollection<Study>(allStudies);
        StudiesList.ItemsSource = _studyCollection;
    }
    

    private void OpenCreateStudyWindow(object sender, RoutedEventArgs e)
    {
        StudyCreation studyCreation = new StudyCreation();
        studyCreation.Owner = this;
        studyCreation.ShowDialog();
    }

    private void EditButtonClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Under Construction");
    }
}