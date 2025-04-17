using System.Windows;

namespace CIMEX_Project;

public partial class StudyCreation : Window
{
    private PrincipalInvestigator _principalInvestigator;
    private List<Visit> _visits;
    public StudyCreation()
    {
        InitializeComponent();
    }

    private void CreateStudy(object sender, RoutedEventArgs e)
    {
       
    }

    private void CreatePrincipalInvestigator(object sender, RoutedEventArgs e)
    {
        var form = new TeamMemberInputForm(true);
        if (form.ShowDialog() == true)
        {
           _principalInvestigator = new PrincipalInvestigator(form.Result.Name,
                form.Result.Surname, form.Result.Email, "Medical Doctor");
            
            PrincipalInvestigatorData.Text = $"Principal Investigator:\n" +
                                             $"Dr.{_principalInvestigator.Surname} {_principalInvestigator.Name} " +
                                             $"e-Mail {_principalInvestigator.Email}";
        }
        
    }

    private void AddVisit(object sender, RoutedEventArgs e)
    {
        var form = new VisitAddition();
        if (form.ShowDialog() == true)
        {
            
        }
    }
}