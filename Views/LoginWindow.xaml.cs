using System;
using System.Windows;
using DueskWPF.Services;

namespace DueskWPF.Views;

public partial class LoginWindow : Window
{
    private readonly ApiService _apiService = new();
    
    public LoginWindow()
    {
        InitializeComponent();
        
        // Enter key handling
        PasswordBox.KeyDown += (s, e) => 
        { 
            if (e.Key == System.Windows.Input.Key.Enter) 
                _ = LoginAsync(); 
        };
        UsernameBox.KeyDown += (s, e) => 
        { 
            if (e.Key == System.Windows.Input.Key.Enter) 
                _ = LoginAsync(); 
        };
    }
    
    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        await LoginAsync();
    }
    
    private async Task LoginAsync()
    {
        LoginButton.IsEnabled = false;
        ErrorText.Visibility = Visibility.Collapsed;
        
        var username = UsernameBox.Text.Trim();
        var password = PasswordBox.Password;
        
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Bitte Benutzername und Passwort eingeben");
            LoginButton.IsEnabled = true;
            return;
        }
        
        try
        {
            var response = await _apiService.LoginAsync(username, password);
            
            if (response.Success && !string.IsNullOrEmpty(response.UserID) && 
                !string.IsNullOrEmpty(response.Session))
            {
                SessionManager.Instance.SetSession(response.UserID, response.Session);
                
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                ShowError(response.Error ?? "Anmeldung fehlgeschlagen");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Verbindungsfehler: {ex.Message}");
        }
        finally
        {
            LoginButton.IsEnabled = true;
        }
    }
    
    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.Visibility = Visibility.Visible;
    }
}