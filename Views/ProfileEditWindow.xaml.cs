using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DueskWPF.Models;
using DueskWPF.Services;

namespace DueskWPF.Views;

public partial class ProfileEditWindow : Window
{
    private readonly Profile? _editProfile;
    private readonly ApiService _apiService = new();
    private List<Group> _groups = new();
    private List<int> _seItems = new List<int>(Enumerable.Repeat(2, 36));
    private List<int> _feItems = new List<int>(Enumerable.Repeat(2, 36));
    private List<ItemRating> _seRatings = new();
    private List<ItemRating> _feRatings = new();
    
    public ProfileEditWindow(Profile? profile = null)
    {
        InitializeComponent();
        _editProfile = profile;
        
        if (profile != null)
        {
            NameBox.Text = profile.Name;
            _seItems = profile.GetAllSEItems();
            _feItems = profile.GetAllFEItems();
        }
        
        Loaded += async (s, e) => await LoadGroupsAsync();
        InitializeItemsControls();
    }
    
    private async Task LoadGroupsAsync()
    {
        _groups = await _apiService.GetGroupsAsync();
        
        var items = new List<object> { "Keine Gruppe" };
        items.AddRange(_groups.Select(g => g.Name));
        items.Add("+ Neue Gruppe...");
        
        GroupCombo.ItemsSource = items;
        GroupCombo.SelectedIndex = 0;
        
        if (_editProfile?.Gruppename != null)
        {
            var index = items.FindIndex(x => x.ToString() == _editProfile.Gruppename);
            if (index >= 0) GroupCombo.SelectedIndex = index;
        }
    }
    
    private void InitializeItemsControls()
    {
        for (int i = 0; i < 36; i++)
        {
            _seRatings.Add(new ItemRating { Index = i, Name = Norms.Items[i], Value = _seItems[i] });
            _feRatings.Add(new ItemRating { Index = i, Name = Norms.Items[i], Value = _feItems[i] });
        }
        
        SEItemsControl.ItemsSource = _seRatings;
        FEItemsControl.ItemsSource = _feRatings;
    }
    
    private void GroupCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = GroupCombo.SelectedItem?.ToString();
        NewGroupPanel.Visibility = selected == "+ Neue Gruppe..." ? Visibility.Visible : Visibility.Collapsed;
    }
    
    private void Rating_Checked(object sender, RoutedEventArgs e)
    {
        var radio = sender as RadioButton;
        var tag = radio?.Tag?.ToString();
        var value = int.TryParse(tag, out var v) ? v : 2;
        
        var parent = FindParent<Border>(radio);
        if (parent?.DataContext is ItemRating rating)
        {
            rating.Value = value;
            
            if (FindParent<ItemsControl>(radio) == SEItemsControl)
                _seItems[rating.Index] = value;
            else
                _feItems[rating.Index] = value;
        }
    }
    
    private T? FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        var parent = System.Windows.Media.VisualTreeHelper.GetParent(child);
        while (parent != null && !(parent is T))
            parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
        return parent as T;
    }
    
    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var name = NameBox.Text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show("Bitte geben Sie einen Namen ein", "Fehler", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        
        SaveButton.IsEnabled = false;
        
        int? groupId = null;
        var selectedGroup = GroupCombo.SelectedItem?.ToString();
        
        if (selectedGroup == "+ Neue Gruppe..." && !string.IsNullOrEmpty(NewGroupBox.Text))
        {
            var createSuccess = await _apiService.CreateGroupAsync(NewGroupBox.Text.Trim());
            if (createSuccess)
            {
                _groups = await _apiService.GetGroupsAsync();
                var newGroup = _groups.FirstOrDefault(g => g.Name == NewGroupBox.Text.Trim());
                if (newGroup != null) groupId = newGroup.GruppeID;
            }
        }
        else if (selectedGroup != null && selectedGroup != "Keine Gruppe" && selectedGroup != "+ Neue Gruppe...")
        {
            var group = _groups.FirstOrDefault(g => g.Name == selectedGroup);
            if (group != null) groupId = group.GruppeID;
        }
        
        var profileData = new ProfileCreate
        {
            Name = name,
            GruppeID = groupId,
            Gruppename = selectedGroup?.StartsWith("+") == true ? NewGroupBox.Text : selectedGroup
        };
        
        // Set all 72 items
        for (int i = 0; i < 36; i++)
        {
            typeof(ProfileCreate).GetProperty($"Item{i + 1}")?.SetValue(profileData, _seItems[i]);
            typeof(ProfileCreate).GetProperty($"Feitem{i + 1}")?.SetValue(profileData, _feItems[i]);
        }
        
        bool saveSuccess;
        if (_editProfile != null)
            saveSuccess = await _apiService.UpdateProfileAsync(_editProfile);
        else
            saveSuccess = await _apiService.CreateProfileAsync(profileData);
        
        if (saveSuccess)
        {
            DialogResult = true;
            Close();
        }
        else
        {
            MessageBox.Show("Fehler beim Speichern des Profils", "Fehler", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            SaveButton.IsEnabled = true;
        }
    }
    
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
    
    private class ItemRating
    {
        public int Index { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}