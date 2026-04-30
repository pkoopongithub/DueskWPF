using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DueskWPF.Models;
using DueskWPF.Services;

namespace DueskWPF.Views;

public partial class GroupManagerWindow : Window
{
    private readonly ApiService _apiService = new();
    private List<Group> _groups = new();
    
    public GroupManagerWindow()
    {
        InitializeComponent();
        Loaded += async (s, e) => await LoadGroupsAsync();
    }
    
    private async Task LoadGroupsAsync()
    {
        _groups = await _apiService.GetGroupsAsync();
        GroupListBox.ItemsSource = _groups;
    }
    
    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var name = NewGroupBox.Text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show("Bitte geben Sie einen Namen ein", "Hinweis", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        
        AddButton.IsEnabled = false;
        
        var success = await _apiService.CreateGroupAsync(name);
        if (success)
        {
            NewGroupBox.Text = "";
            await LoadGroupsAsync();
        }
        else
        {
            MessageBox.Show("Fehler beim Erstellen der Gruppe", "Fehler", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        AddButton.IsEnabled = true;
    }
    
    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as System.Windows.Controls.Button;
        var id = button?.Tag as int?;
        
        if (id.HasValue)
        {
            var result = MessageBox.Show("Möchten Sie diese Gruppe wirklich löschen?", 
                "Gruppe löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                var success = await _apiService.DeleteGroupAsync(id.Value);
                if (success)
                    await LoadGroupsAsync();
                else
                    MessageBox.Show("Fehler beim Löschen der Gruppe", "Fehler", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}