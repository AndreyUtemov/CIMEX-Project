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
        MainWindowManagement mainWindowManagement = new MainWindowManagement();
        
        string userLogin = LoginBox.Text;
        string password = PasswordBox.Password;
        bool accessApproved = await mainWindowManagement.CheckUser(userLogin, password);

        if (accessApproved)
        {
            await mainWindowManagement.ProgrammStart(userLogin);
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