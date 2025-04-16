using System.Windows;

namespace CIMEX_Project;

public partial class TeamMemberInputForm : Window
{
    public TeamMember Result { get; private set; }
    
    public TeamMemberInputForm()
    {
        InitializeComponent();
    }

    private void CreateParticipant(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EMail.Text) || string.IsNullOrWhiteSpace(Name.Text) ||
            string.IsNullOrWhiteSpace(Surname.Text))
        {
            MessageBox.Show("Enter all data", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
           Result = new TeamMember
           {
               Email = EMail.Text,
               Name = Name.Text,
               Surname = Surname.Text,
               Role = "Medical Doctor"
           };
        }

        this.DialogResult = true;
        this.Close();
    }
}