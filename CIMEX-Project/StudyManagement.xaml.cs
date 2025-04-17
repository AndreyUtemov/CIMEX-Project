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
        if (string.IsNullOrWhiteSpace(EMail.Text) || string.IsNullOrWhiteSpace(Name.Text) ||
            string.IsNullOrWhiteSpace(Surname.Text)
            || !(RoleBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.IsEnabled))
        {
            MessageBox.Show("Enter all data", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
            string email = EMail.Text;
            string name = Name.Text;
            string surname = Surname.Text;
            string role = (RoleBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            TeamMember teamMember = new TeamMember(name, surname, email, role);
            _user.SetTeamMemeber(teamMember, _study);
        }
    }

    private void WithdrawButtonClick(object sender, RoutedEventArgs routedEventArgs)
    {
        if (sender is Button button && button.Tag is string email)
        {
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to withdraw this member?",
                "Confirm Delete",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                _user.WithdrawTeamMember(email, _study);
                TeamMembersList.Items.Refresh(); // Обновляем отображение
            }
        }
    }

    private async Task GetTeam()
    {
        List<TeamMember> teamOfStudy = await _user.GetTeamList(_study);
        _team = teamOfStudy;
    }
}