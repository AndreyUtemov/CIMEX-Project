using System.Windows;
using System.Windows.Controls;

namespace CIMEX_Project;

public partial class StudyManagement : Window
{
    public string StudyTitle { get; set; }
    private PrincipalInvestigator _user;
    private Study _study;
    private List<TeamMember> _team;

    public StudyManagement(TeamMember user, Study study)
    {
        InitializeComponent();
        _user = (PrincipalInvestigator?)user;
        _study = study;
        GetTeam();
        StudyTitle = $"{study.StudyName} team management";
        DataContext = this;
    }

    private void CreateParticipant(object sender, RoutedEventArgs routedEventArgs)
    {
        TeamMemberInputForm teamMemberInputForm = new TeamMemberInputForm(false);
        teamMemberInputForm.Owner = this;
        teamMemberInputForm.ShowDialog();
        

    }

    private void WithdrawButtonClick(object sender, RoutedEventArgs routedEventArgs)
    {
        if (sender is Button button && button.Tag is string email)
        {
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to withdraw this team member?",
                "Confirm Delete",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                _user.WithdrawTeamMember(email, _study);
                TeamMembersList.Items.Refresh(); 
            }
        }
    }

    private async Task GetTeam()
    {
        List<TeamMember> teamOfStudy = await _user.GetTeamList(_study);
        _team = teamOfStudy;
    }
}