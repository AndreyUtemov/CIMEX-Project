using System.Windows;

namespace CIMEX_Project;

public partial class AdminWindow : Window
{
    public AdminWindow()
    {
        InitializeComponent();
        Loaded += AdminWindow_Loaded; // ждём полной загрузки окна
    }
    
    private async void AdminWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await SetButtons(); // корректно дожидаемся кнопок
        
    }
    
    private void AddButtons(List<Button> upperButtons)
    {
        UpperPanel.Children.Clear();
        Console.WriteLine("Creating buttons for ui");

        foreach (Button button in upperButtons)
        {
            Console.WriteLine($"Create button {button.Content.ToString()}");
            button.Click -= Study_Button_Click;
            button.Click += Study_Button_Click;
            UpperPanel.Children.Add(button);
        }
    }

    private void OpenCreateStudyWindow(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}