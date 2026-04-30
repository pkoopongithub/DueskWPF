using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DueskWPF.Models;
using DueskWPF.Services;

namespace DueskWPF.Views;

public partial class MainWindow : Window
{
    private readonly ApiService _apiService = new();
    private List<Profile> _profiles = new();
    private List<Group> _groups = new();
    private Group? _selectedGroup;
    
    public MainWindow()
    {
        InitializeComponent();
        Loaded += async (s, e) => await LoadDataAsync();
        GroupListBox.SelectionChanged += GroupListBox_SelectionChanged;
    }
    
    private async Task LoadDataAsync()
    {
        SetStatus("Lade Profile...");
        
        try
        {
            _profiles = await _apiService.GetProfilesAsync();
            _groups = await _apiService.GetGroupsAsync();
            
            UpdateGroupList();
            UpdateProfileList();
            
            SetStatus($"{_profiles.Count} Profile geladen");
        }
        catch (Exception ex)
        {
            SetStatus($"Fehler: {ex.Message}");
            MessageBox.Show($"Daten konnten nicht geladen werden: {ex.Message}", 
                "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void UpdateGroupList()
    {
        var items = new List<Group> { new Group { GruppeID = 0, Name = "📁 Alle Profile" } };
        items.AddRange(_groups);
        GroupListBox.ItemsSource = items;
        
        if (_selectedGroup == null && items.Count > 0)
            GroupListBox.SelectedItem = items[0];
    }
    
    private void UpdateProfileList()
    {
        var filtered = _profiles.AsEnumerable();
        
        if (_selectedGroup != null && _selectedGroup.GruppeID > 0)
            filtered = filtered.Where(p => p.GruppeID == _selectedGroup.GruppeID.ToString());
        
        if (!string.IsNullOrEmpty(SearchBox.Text))
        {
            var search = SearchBox.Text.ToLower();
            filtered = filtered.Where(p => p.Name.ToLower().Contains(search) ||
                (p.Gruppename?.ToLower().Contains(search) ?? false));
        }
        
        ProfileListBox.ItemsSource = filtered.ToList();
    }
    
    private void SetStatus(string message) => StatusText.Text = message;
    
    private void GroupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedGroup = GroupListBox.SelectedItem as Group;
        UpdateProfileList();
    }
    
    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => UpdateProfileList();
    
    private async void RefreshButton_Click(object sender, RoutedEventArgs e) => await LoadDataAsync();
    
    private void GroupsButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new GroupManagerWindow();
        dialog.Owner = this;
        dialog.ShowDialog();
        _ = LoadDataAsync();
    }
    
    private void TimeSeriesButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedGroup = GroupListBox.SelectedItem as Group;
        var groupName = selectedGroup?.Name?.Replace("📁 ", "") ?? "Alle Profile";
        var dialog = new TimeSeriesWindow(groupName);
        dialog.Owner = this;
        dialog.ShowDialog();
    }
    
    private void NewProfileButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ProfileEditWindow();
        dialog.Owner = this;
        if (dialog.ShowDialog() == true)
            _ = LoadDataAsync();
    }
    
    private async void ProfileAction_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var action = button?.Tag as string;
        var profile = (button?.DataContext as Profile);
        
        if (profile == null) return;
        
        switch (action)
        {
            case "view":
                var detailWindow = new ProfileDetailWindow(profile);
                detailWindow.Owner = this;
                detailWindow.ShowDialog();
                break;
                
            case "edit":
                var editWindow = new ProfileEditWindow(profile);
                editWindow.Owner = this;
                if (editWindow.ShowDialog() == true)
                    await LoadDataAsync();
                break;
                
            case "delete":
                var result = MessageBox.Show($"Möchten Sie \"{profile.Name}\" wirklich löschen?", 
                    "Profil löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var success = await _apiService.DeleteProfileAsync(profile.ProfilID);
                    if (success)
                        await LoadDataAsync();
                    else
                        MessageBox.Show("Löschen fehlgeschlagen", "Fehler", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
                break;
        }
    }
    
    private void ProfileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
    
    private void ProfileListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var profile = ProfileListBox.SelectedItem as Profile;
        if (profile != null)
        {
            // Debug-Ausgabe um sicherzustellen dass Profile nicht null ist
            System.Diagnostics.Debug.WriteLine($"Opening detail for profile: {profile.Name}, ID: {profile.ProfilID}");
            
            var detailWindow = new ProfileDetailWindow(profile);
            detailWindow.Owner = this;
            detailWindow.ShowDialog();
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("ProfileListBox_MouseDoubleClick: SelectedItem is not a Profile");
        }
    }
    
    private void Logo_Click(object sender, MouseButtonEventArgs e)
    {
        _ = LoadDataAsync();
    }
    
    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        SessionManager.Instance.Clear();
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }
}