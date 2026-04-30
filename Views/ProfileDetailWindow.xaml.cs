using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using DueskWPF.Models;
using DueskWPF.Services;

namespace DueskWPF.Views;

public partial class ProfileDetailWindow : Window
{
    private readonly Profile _profile;
    private string _normType = "HS";
    private int[] _seValues = Array.Empty<int>();
    private int[] _feValues = Array.Empty<int>();
    private bool _isInitialized = false;
    
    public ProfileDetailWindow(Profile profile)
    {
        InitializeComponent();
        
        // NULL-CHECK für Profile
        if (profile == null)
        {
            System.Diagnostics.Debug.WriteLine("ProfileDetailWindow: profile is null!");
            MessageBox.Show("Fehler: Profil konnte nicht geladen werden.", "Fehler", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
            return;
        }
        
        _profile = profile;
        
        NameText.Text = profile.Name ?? "Unbekannt";
        InfoText.Text = $"Gruppe: {profile.Gruppename ?? "Keine Gruppe"} | ID: {profile.ProfilID ?? "?"}";
        
        // Warten bis das Fenster geladen ist, bevor Werte berechnet werden
        this.Loaded += ProfileDetailWindow_Loaded;
    }
    
    private void ProfileDetailWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _isInitialized = true;
        CalculateValues();
    }
    
    private void CalculateValues()
    {
        if (!_isInitialized) return;
        
        // Prüfen ob Controls existieren
        if (SETable == null || FETable == null || SEChart == null || FEChart == null || 
            ComparisonChart == null || ItemsTable == null)
        {
            System.Diagnostics.Debug.WriteLine("CalculateValues: Some controls are still null");
            return;
        }
        
        try
        {
            var (se, fe) = Calculator.CalculateCompetenceValues(_profile, _normType);
            _seValues = se ?? new int[6];
            _feValues = fe ?? new int[6];
            
            RenderSETable();
            RenderFETable();
            RenderSEChart();
            RenderFEChart();
            RenderComparisonChart();
            RenderStatistics();
            RenderItemsTable();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CalculateValues Error: {ex.Message}");
            MessageBox.Show($"Fehler bei der Berechnung: {ex.Message}", "Fehler", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void RenderSETable()
    {
        // Prüfen ob SETable existiert
        if (SETable == null)
        {
            System.Diagnostics.Debug.WriteLine("RenderSETable: SETable is null");
            return;
        }
        
        var data = new List<CompetenceRow>();
        for (int i = 0; i < 6; i++)
        {
            int value = i < _seValues.Length ? _seValues[i] : 3;
            data.Add(new CompetenceRow
            {
                Competence = Norms.Competencies[i],
                Level1 = value == 1 ? "X" : "",
                Level2 = value == 2 ? "X" : "",
                Level3 = value == 3 ? "X" : "",
                Level4 = value == 4 ? "X" : "",
                Level5 = value == 5 ? "X" : "",
                Rating = Norms.GetRatingText(value)
            });
        }
        
        SETable.ItemsSource = data;
        SetupTableColumns(SETable);
    }
    
    private void RenderFETable()
    {
        // Prüfen ob FETable existiert
        if (FETable == null)
        {
            System.Diagnostics.Debug.WriteLine("RenderFETable: FETable is null");
            return;
        }
        
        var data = new List<CompetenceRow>();
        for (int i = 0; i < 6; i++)
        {
            int value = i < _feValues.Length ? _feValues[i] : 3;
            data.Add(new CompetenceRow
            {
                Competence = Norms.Competencies[i],
                Level1 = value == 1 ? "X" : "",
                Level2 = value == 2 ? "X" : "",
                Level3 = value == 3 ? "X" : "",
                Level4 = value == 4 ? "X" : "",
                Level5 = value == 5 ? "X" : "",
                Rating = Norms.GetRatingText(value)
            });
        }
        
        FETable.ItemsSource = data;
        SetupTableColumns(FETable);
    }
    
    private void SetupTableColumns(DataGrid table)
    {
        if (table == null) return;
        
        table.Columns.Clear();
        table.Columns.Add(new DataGridTextColumn { Header = "Kompetenz", Binding = new System.Windows.Data.Binding("Competence"), Width = 150 });
        table.Columns.Add(new DataGridTextColumn { Header = "1", Binding = new System.Windows.Data.Binding("Level1"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        table.Columns.Add(new DataGridTextColumn { Header = "2", Binding = new System.Windows.Data.Binding("Level2"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        table.Columns.Add(new DataGridTextColumn { Header = "3", Binding = new System.Windows.Data.Binding("Level3"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        table.Columns.Add(new DataGridTextColumn { Header = "4", Binding = new System.Windows.Data.Binding("Level4"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        table.Columns.Add(new DataGridTextColumn { Header = "5", Binding = new System.Windows.Data.Binding("Level5"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        table.Columns.Add(new DataGridTextColumn { Header = "Bewertung", Binding = new System.Windows.Data.Binding("Rating"), Width = 130 });
    }
    
    private void RenderSEChart()
    {
        if (SEChart == null)
        {
            System.Diagnostics.Debug.WriteLine("RenderSEChart: SEChart is null");
            return;
        }
        
        SEChart.Series = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Selbsteinschätzung",
                Values = new ChartValues<int>(_seValues),
                LineSmoothness = 0.5,
                PointGeometrySize = 12,
                StrokeThickness = 3
            }
        };
        SEChart.AxisX.Clear();
        SEChart.AxisY.Clear();
        SEChart.AxisX.Add(new Axis { Title = "Kompetenz", Labels = Norms.Competencies });
        SEChart.AxisY.Add(new Axis { Title = "Wert", MinValue = 1, MaxValue = 5, LabelFormatter = v => v.ToString("0") });
        SEChart.LegendLocation = LegendLocation.Top;
    }
    
    private void RenderFEChart()
    {
        if (FEChart == null)
        {
            System.Diagnostics.Debug.WriteLine("RenderFEChart: FEChart is null");
            return;
        }
        
        FEChart.Series = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Fremdeinschätzung",
                Values = new ChartValues<int>(_feValues),
                LineSmoothness = 0.5,
                PointGeometrySize = 12,
                StrokeThickness = 3
            }
        };
        FEChart.AxisX.Clear();
        FEChart.AxisY.Clear();
        FEChart.AxisX.Add(new Axis { Title = "Kompetenz", Labels = Norms.Competencies });
        FEChart.AxisY.Add(new Axis { Title = "Wert", MinValue = 1, MaxValue = 5, LabelFormatter = v => v.ToString("0") });
        FEChart.LegendLocation = LegendLocation.Top;
    }
    
    private void RenderComparisonChart()
    {
        if (ComparisonChart == null)
        {
            System.Diagnostics.Debug.WriteLine("RenderComparisonChart: ComparisonChart is null");
            return;
        }
        
        ComparisonChart.Series = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Selbsteinschätzung (SE)",
                Values = new ChartValues<int>(_seValues),
                LineSmoothness = 0.5,
                PointGeometrySize = 10,
                StrokeThickness = 2
            },
            new LineSeries
            {
                Title = "Fremdeinschätzung (FE)",
                Values = new ChartValues<int>(_feValues),
                LineSmoothness = 0.5,
                PointGeometrySize = 10,
                StrokeThickness = 2,
                StrokeDashArray = new System.Windows.Media.DoubleCollection { 5, 5 }
            }
        };
        ComparisonChart.AxisX.Clear();
        ComparisonChart.AxisY.Clear();
        ComparisonChart.AxisX.Add(new Axis { Title = "Kompetenz", Labels = Norms.Competencies });
        ComparisonChart.AxisY.Add(new Axis { Title = "Wert", MinValue = 1, MaxValue = 5, LabelFormatter = v => v.ToString("0") });
        ComparisonChart.LegendLocation = LegendLocation.Top;
    }
    
    private void RenderStatistics()
    {
        if (CorrelationText == null || AgreementText == null || CorrelationDesc == null || 
            AgreementDesc == null || InterpretationText == null)
        {
            System.Diagnostics.Debug.WriteLine("RenderStatistics: Some stat controls are null");
            return;
        }
        
        var correlation = Calculator.CalculateCorrelation(_seValues, _feValues);
        var agreement = Calculator.CalculateAgreement(_profile.GetAllSEItems(), _profile.GetAllFEItems());
        
        CorrelationText.Text = correlation.ToString("F2");
        AgreementText.Text = $"{agreement:F1}%";
        
        string corrDesc;
        if (correlation >= 0.8) corrDesc = "sehr gute Übereinstimmung";
        else if (correlation >= 0.6) corrDesc = "gute Übereinstimmung";
        else if (correlation >= 0.4) corrDesc = "mäßige Übereinstimmung";
        else if (correlation >= 0.2) corrDesc = "schwache Übereinstimmung";
        else corrDesc = "keine Übereinstimmung";
        CorrelationDesc.Text = corrDesc;
        
        string agreeDesc;
        if (agreement >= 80) agreeDesc = "hohe inhaltliche Übereinstimmung";
        else if (agreement >= 60) agreeDesc = "mittlere inhaltliche Übereinstimmung";
        else if (agreement >= 40) agreeDesc = "geringe inhaltliche Übereinstimmung";
        else agreeDesc = "sehr geringe inhaltliche Übereinstimmung";
        AgreementDesc.Text = agreeDesc;
        
        var interpretation = Calculator.GetInterpretation(correlation, agreement, _seValues, _feValues);
        InterpretationText.Text = interpretation;
    }
    
    private void RenderItemsTable()
    {
        if (ItemsTable == null)
        {
            System.Diagnostics.Debug.WriteLine("RenderItemsTable: ItemsTable is null");
            return;
        }
        
        var items = new List<ItemRow>();
        var seItems = _profile.GetAllSEItems();
        var feItems = _profile.GetAllFEItems();
        
        for (int i = 0; i < 36; i++)
        {
            items.Add(new ItemRow
            {
                Index = i + 1,
                Name = Norms.Items[i],
                SE = seItems != null && i < seItems.Count ? seItems[i] : 2,
                FE = feItems != null && i < feItems.Count ? feItems[i] : 2
            });
        }
        
        ItemsTable.ItemsSource = items;
    }
    
    private void NormType_Changed(object sender, RoutedEventArgs e)
    {
        if (!_isInitialized) return;
        _normType = HSRadio.IsChecked == true ? "HS" : "FS";
        CalculateValues();
    }
    
    private class CompetenceRow
    {
        public string Competence { get; set; } = string.Empty;
        public string Level1 { get; set; } = string.Empty;
        public string Level2 { get; set; } = string.Empty;
        public string Level3 { get; set; } = string.Empty;
        public string Level4 { get; set; } = string.Empty;
        public string Level5 { get; set; } = string.Empty;
        public string Rating { get; set; } = string.Empty;
    }
    
    private class ItemRow
    {
        public int Index { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SE { get; set; }
        public int FE { get; set; }
    }
}