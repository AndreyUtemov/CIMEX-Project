using System.Windows;

namespace CIMEX_Project;

public partial class LoginWindow : Window
{
    
    public LoginWindow()
    {
        InitializeComponent();
    }

    private async void LoginButtonClick(object sender, RoutedEventArgs e)
    {
        
        string userLogin = LoginBox.Text;
        string password = PasswordBox.Password;

        if (userLogin == "admin" && password == "12345678")
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Close();
        }
        else
        {
            bool accessApproved = await CheckUser(userLogin, password);

            if (accessApproved)
            {
                MainWindowManagement mainWindowManagement = new MainWindowManagement();
                await mainWindowManagement.SetUser(userLogin);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong Login or Password", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
    public async Task<bool> CheckUser(string login, string password)
    {
        try
        {
            DAOTeamMemeberNeo4j daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();
            bool accessApproved = await daoTeamMemeberNeo4J.CheckUserPassword(login, password);
            return accessApproved;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}


