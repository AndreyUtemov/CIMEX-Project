using System.Windows;

namespace CIMEX_Project;

public partial class StudyManagement : Window
{
    public string StudyTitle { get; set; }
    private PrincipalInvestigator _user;
    private Study _study;
    public StudyManagement(TeamMember user, Study study)
    {
        _user = (PrincipalInvestigator?)user;
        _study = study;
        List<TeamMember> teamOfStudy = user.GetTeamList(_study);
        InitializeComponent();
        StudyTitle = $"{study.StudyName} team management";
        DataContext = this;
    }
}