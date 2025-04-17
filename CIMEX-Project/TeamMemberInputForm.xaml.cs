using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CIMEX_Project;

public partial class TeamMemberInputForm : Window
{
    public TeamMember Result { get; private set; }
    private ObservableCollection<TeamMember> _teamMembers = new ObservableCollection<TeamMember>();
    
    public TeamMemberInputForm(bool isAdmin)
    {
     InitializeComponent();
     
     if (!isAdmin)
     {
         RoleBox.Visibility = Visibility.Visible;
     }
     CreateListOfTeamMemebers();
    }

    private void CreateParticipant(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EMailBox.Text) || string.IsNullOrWhiteSpace(NameBox.Text) ||
            string.IsNullOrWhiteSpace(SurnameBox.Text))
        {
            MessageBox.Show("Enter all data", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
           Result = new TeamMember
           {
               Email = EMailBox.Text,
               Name = NameBox.Text,
               Surname = SurnameBox.Text,
           };
        }

        this.DialogResult = true;
        this.Close();
    }

    private void AddButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is TeamMember member)
        {
            EMailBox.Text = member.Email;
            NameBox.Text = member.Name;
            SurnameBox.Text = member.Surname;
        }
    }

    private async void CreateListOfTeamMemebers()
    {
        Console.WriteLine("CreateListOfTeamMembers started");
        AdminDao adminDao = new AdminDao();
        var fullTeam = await adminDao.GetAllTeamMember();
        List<TeamMember> team = new List<TeamMember>();
        foreach (var teamMemeber in fullTeam)
        {
            Console.WriteLine($"{teamMemeber.Surname}  {teamMemeber.Role}");
            if (teamMemeber.Role == "Study Investigator" || teamMemeber.Role == "Medical Doctor")
            {
                team.Add(teamMemeber);
            }
        }
        
        _teamMembers = new ObservableCollection<TeamMember>(team);
        TeamMembersList.ItemsSource = _teamMembers;

    }
}