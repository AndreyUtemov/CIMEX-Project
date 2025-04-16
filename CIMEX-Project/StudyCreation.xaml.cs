using System.Windows;

namespace CIMEX_Project;

public partial class StudyCreation : Window
{
    public StudyCreation()
    {
        InitializeComponent();
    }

    private void CreateStudy(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void CreatePrincipalInvestigator(object sender, RoutedEventArgs e)
    {
        var form = new TeamMemberInputForm();
        if (form.ShowDialog() == true)
        {
            PrincipalInvestigator principalInvestigator = (PrincipalInvestigator)form.Result;
            PrincipalInvestigatorData.Text = $"Principal Investigator:\n" +
                                             $"Dr.{principalInvestigator.Surname} {principalInvestigator.Name}" +
                                             $"e-Mail {principalInvestigator.Email}";
        }
    }

    private void Addvisit(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}