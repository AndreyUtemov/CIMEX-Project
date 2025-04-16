using System.Windows;

namespace CIMEX_Project;

public partial class AdminWindow : Window
{
    public AdminWindow()
    {
        InitializeComponent();
    }
    
    
    

    private void OpenCreateStudyWindow(object sender, RoutedEventArgs e)
    {
        StudyCreation studyCreation = new StudyCreation();
        studyCreation.Owner = this;
        studyCreation.ShowDialog();
    }
}