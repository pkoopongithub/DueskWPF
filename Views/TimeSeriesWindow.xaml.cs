using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using DueskWPF.Models;
using DueskWPF.Services;

namespace DueskWPF.Views;

public partial class TimeSeriesWindow : Window
{
    private readonly string _groupName;
    private readonly ApiService _apiService = new();
    private List<Profile> _profiles = new();
    private List<List<int>> _competenceValues = new();
    private List<string> _profileNames = new();
    
    public TimeSeriesWindow(string groupName)
    {
        InitializeComponent();
        _groupName = groupName;
        GroupNameText.Text = $"Gruppe: {groupName}";
        
        Loaded += async (s, e) => await LoadDataAsync();
    }
    
    private async Task LoadDataAsync()
    {
        var allProfiles = await _apiService.GetProfilesAsync();
        
        // Filter by group
        if (_groupName != "Alle Profile")
            _profiles = allProfiles.Where(p => p.Gruppename == _groupName).ToList();
        else
            _profiles = allProfiles;
        
        // Sort by created date if available
        _profiles = _profiles.OrderBy(p => p.CreatedAt).ToList();
        
        foreach (var profile in _profiles)
        {
            var (se, _) = Calculator.CalculateCompetenceValues(profile);
            _competenceValues.Add(se.ToList()); // Convert int[] to List<int>
            _profileNames.Add(profile.Name);
        }
        
        // Setup competence combo
        CompetenceCombo.ItemsSource = Norms.Competencies;
        CompetenceCombo.SelectedIndex = 0;
        
        RenderChart(0);
        RenderDataTable();
    }
    
    private void RenderChart(int competenceIndex)
    {
        // Convert int[] to List<int> for ChartValues
        var values = _competenceValues.Select(v => v[competenceIndex]).ToList();
        
        TimeSeriesChart.Series = new SeriesCollection
        {
            new LineSeries
            {
                Title = Norms.Competencies[competenceIndex],
                Values = new ChartValues<int>(values), // Now passing List<int>
                LineSmoothness = 0.3,
                PointGeometrySize = 10,
                StrokeThickness = 2
            }
        };
        
        TimeSeriesChart.AxisX.Clear();
        TimeSeriesChart.AxisY.Clear();
        TimeSeriesChart.AxisX.Add(new Axis 
        { 
            Title = "Profile", 
            Labels = _profileNames.Select((n, i) => (i + 1).ToString()).ToList(),
            Separator = new Separator { Step = 1 }
        });
        TimeSeriesChart.AxisY.Add(new Axis { Title = "Wert", MinValue = 1, MaxValue = 5 });
    }
    
    private void RenderDataTable()
    {
        var data = new List<TimeSeriesRow>();
        for (int i = 0; i < _profiles.Count; i++)
        {
            data.Add(new TimeSeriesRow
            {
                Profile = _profileNames[i],
                Arbeitsverhalten = _competenceValues[i][0],
                Lernverhalten = _competenceValues[i][1],
                Sozialverhalten = _competenceValues[i][2],
                Fachkompetenz = _competenceValues[i][3],
                PersonaleKompetenz = _competenceValues[i][4],
                Methodenkompetenz = _competenceValues[i][5]
            });
        }
        
        DataTable.ItemsSource = data;
    }
    
    private void CompetenceCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (CompetenceCombo.SelectedIndex >= 0)
            RenderChart(CompetenceCombo.SelectedIndex);
    }
    
    private class TimeSeriesRow
    {
        public string Profile { get; set; } = string.Empty;
        public int Arbeitsverhalten { get; set; }
        public int Lernverhalten { get; set; }
        public int Sozialverhalten { get; set; }
        public int Fachkompetenz { get; set; }
        public int PersonaleKompetenz { get; set; }
        public int Methodenkompetenz { get; set; }
    }
}