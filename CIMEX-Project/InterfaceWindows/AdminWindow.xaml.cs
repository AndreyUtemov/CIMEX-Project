using System.Collections.ObjectModel;
using System.Windows;

namespace CIMEX_Project;
/*
 * An administrator window that displays existing studies, the ability to create new studies and make changes to current ones
 */
public partial class AdminWindow : Window
{
    private ObservableCollection<Study> _studyCollection = new ObservableCollection<Study>();
    public AdminWindow()
    {
        InitializeComponent();
        CreateListOfStudies();
    }

    /*
     * CreateListOfStudies
     * Getting a list of studies from the database, displaying the list on the screen
     */
    private async void CreateListOfStudies()
    {
        Console.WriteLine("CreateListOfStudies started");
        DaoAdminNeo4J daoAdminNeo4J = new DaoAdminNeo4J();
        var allStudies = await daoAdminNeo4J.GetAllStudy();
        foreach (var study in allStudies)
        {
            Console.WriteLine($"{study.StudyName} in List");
            
        }

        _studyCollection = new ObservableCollection<Study>(allStudies);
        StudiesList.ItemsSource = _studyCollection;
    }
    
/*
 * OpenCreateStudyWindow
 * Create Study button handler. Redirects to the new study creation window.
 */
    private void OpenCreateStudyWindow(object sender, RoutedEventArgs e)
    {
        StudyCreation studyCreation = new StudyCreation();
        studyCreation.Owner = this;
        bool? result = studyCreation.ShowDialog();
        if (result == true)
        {
            CreateListOfStudies(); 
        }
    }
/*
 * EditButtonClick
 * Edit button handler. Redirects to the window for changing the structure of the study.
 */
    private void EditButtonClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Under Construction");
    }
/*
 * EditTeamMembers
 * EditTeam Members Button handler. Redirects to the window for changing the team.
 */
    private void EditTeamMembers(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Under Construction");
    }
/*
 /// ExitProgramm
 * Closes the window and exits the program
 */
    private void ExitProgramm(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}