using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using PasswordGenerator;

namespace CIMEX_Project;

public partial class TeamMemberInputForm : Window
{
    public TeamMember Result { get; private set; }
    private ObservableCollection<TeamMember> _teamMembers = new ObservableCollection<TeamMember>();
    private bool _newTeamMember = true;
    
    public TeamMemberInputForm(bool isAdmin)
    {
     InitializeComponent();
     
     if (!isAdmin)
     {
         RoleBox.Visibility = Visibility.Visible;
     }
     CreateListOfTeamMemebers();
    }

    private async void CreateParticipant(object sender, RoutedEventArgs e)
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
               Role = "Clinical investigator"
           };
           if (_newTeamMember)
           {
               string password = CreatePassword();
               MessageBox.Show($"Password for TeamMember {Result.Email} created: {password}","Password created", MessageBoxButton.OK);
               DAOTeamMemeberNeo4j daoTeamMember = new DAOTeamMemeberNeo4j();
               bool teamMemberCreated = await  daoTeamMember.CreateTeamMemeber(Result, password);
               if (teamMemberCreated)
               {
                   MessageBox.Show("New team member added", "Team member added", MessageBoxButton.OK);
               }
           }
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
            _newTeamMember = false;
        }
    }

    private async void CreateListOfTeamMemebers()
    {
        Console.WriteLine("CreateListOfTeamMembers started");
        DAOAdminNeo4j daoAdminNeo4J = new DAOAdminNeo4j();
        var fullTeam = await daoAdminNeo4J.GetAllTeamMember();
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

    private string CreatePassword()
    {
        var password = new Password()
            .IncludeLowercase()
            .IncludeUppercase()
            .IncludeNumeric()
            .LengthRequired(10);
        string result = password.Next();
        Console.WriteLine($"Password {result}");
        return result;
    }
}