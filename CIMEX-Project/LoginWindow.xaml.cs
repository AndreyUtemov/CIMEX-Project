using System.Windows;

namespace CIMEX_Project;

public partial class LoginWindow : Window
{
    public static string secretUserLogin = "a";
    public static string secretPassword = "a";

    public LoginWindow()
    {
        InitializeComponent();
    }

    private async void LoginButtonClick(object sender, RoutedEventArgs e)
    {
        string userLogin = LoginBox.Text;
        string password = PasswordBox.Password;

        if (userLogin == secretUserLogin && password == secretPassword)
        {
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