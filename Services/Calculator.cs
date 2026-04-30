using System;
using System.Collections.Generic;
using System.Linq;
using DueskWPF.Models;

namespace DueskWPF.Services;

public static class Calculator
{
    public static int[] CalculateSums(IReadOnlyList<int> items)
    {
        if (items == null || items.Count < 36)
            return new int[7];

        var sums = new int[7];
        
        for (var i = 0; i < 10; i++) sums[1] += items[i];
        for (var i = 10; i < 20; i++) sums[2] += items[i];
        for (var i = 20; i < 28; i++) sums[3] += items[i];
        sums[3] += items[8] + items[9];
        for (var i = 28; i < 36; i++) sums[4] += items[i];
        
        sums[5] = items[0] + items[1] + items[5] + items[6] + items[7] +
                  items[8] + items[9] + items[11] + items[12] + items[13] + items[14];
        
        sums[6] = items[2] + items[3] + items[4] + items[8] + items[9] +
                  items[10] + items[16] + items[17];
        
        return sums;
    }
    
    public static int[] CalculateProfileValues(int[] sums, double[][] norm)
    {
        if (sums == null || norm == null || sums.Length < 7 || norm.Length < 6)
            return new int[6];

        var values = new int[6];
        for (var k = 1; k <= 6; k++)
        {
            var value = 5;
            for (var p = 0; p < 5; p++)
            {
                if (sums[k] < norm[k - 1][p])
                {
                    value = p + 1;
                    break;
                }
            }
            values[k - 1] = value;
        }
        return values;
    }
    
    public static double CalculateCorrelation(int[] seValues, int[] feValues)
    {
        if (seValues == null || feValues == null || seValues.Length == 0 || feValues.Length == 0)
            return 0;

        var n = seValues.Length;
        double sumSE = 0, sumFE = 0, sumSEFE = 0, sumSE2 = 0, sumFE2 = 0;
        
        for (var i = 0; i < n; i++)
        {
            sumSE += seValues[i];
            sumFE += feValues[i];
            sumSEFE += seValues[i] * feValues[i];
            sumSE2 += seValues[i] * seValues[i];
            sumFE2 += feValues[i] * feValues[i];
        }
        
        var numerator = n * sumSEFE - sumSE * sumFE;
        var denominator = Math.Sqrt((n * sumSE2 - sumSE * sumSE) * (n * sumFE2 - sumFE * sumFE));
        
        return Math.Abs(denominator) < 0.0001 ? 0 : numerator / denominator;
    }
    
    public static double CalculateAgreement(IReadOnlyList<int> seItems, IReadOnlyList<int> feItems)
    {
        if (seItems == null || feItems == null || seItems.Count == 0 || feItems.Count == 0)
            return 0;

        var matches = 0;
        for (var i = 0; i < 36; i++)
        {
            if (seItems[i] == feItems[i]) matches++;
        }
        return matches * 100.0 / 36;
    }
    
    public static (int[] SE, int[] FE) CalculateCompetenceValues(Profile profile, string normType = "HS")
    {
        // NULL-CHECK: Profile darf nicht null sein
        if (profile == null)
        {
            System.Diagnostics.Debug.WriteLine("CalculateCompetenceValues: profile is null");
            return (new int[6], new int[6]);
        }
        
        var seItems = profile.GetAllSEItems();
        var feItems = profile.GetAllFEItems();
        
        // NULL-CHECK: Items dürfen nicht null sein
        if (seItems == null || feItems == null)
        {
            System.Diagnostics.Debug.WriteLine($"CalculateCompetenceValues: seItems null? {seItems == null}, feItems null? {feItems == null}");
            return (new int[6], new int[6]);
        }
        
        // Prüfen, ob alle 36 Items vorhanden sind
        if (seItems.Count != 36 || feItems.Count != 36)
        {
            System.Diagnostics.Debug.WriteLine($"CalculateCompetenceValues: Invalid item count - SE: {seItems.Count}, FE: {feItems.Count}");
            
            // Items korrigieren wenn nötig
            while (seItems.Count < 36) seItems.Add(2);
            while (feItems.Count < 36) feItems.Add(2);
        }
        
        var seSums = CalculateSums(seItems);
        var feSums = CalculateSums(feItems);
        
        var normSE = normType == "HS" ? Norms.NormSE_HS : Norms.NormSE_FS;
        var normFE = normType == "HS" ? Norms.NormFE_HS : Norms.NormFE_FS;
        
        return (CalculateProfileValues(seSums, normSE), CalculateProfileValues(feSums, normFE));
    }
    
    public static string GetInterpretation(double correlation, double agreement, int[] seValues, int[] feValues)
    {
        var text = "";
        
        if (correlation >= 0.8)
            text += $"Sehr gute Übereinstimmung zwischen Selbst- und Fremdeinschätzung (r = {correlation:F2}).\n\n";
        else if (correlation >= 0.6)
            text += $"Gute Übereinstimmung zwischen Selbst- und Fremdeinschätzung (r = {correlation:F2}).\n\n";
        else if (correlation >= 0.4)
            text += $"Mäßige Übereinstimmung zwischen Selbst- und Fremdeinschätzung (r = {correlation:F2}).\n\n";
        else if (correlation >= 0.2)
            text += $"Schwache Übereinstimmung zwischen Selbst- und Fremdeinschätzung (r = {correlation:F2}).\n\n";
        else
            text += $"Keine signifikante Übereinstimmung zwischen Selbst- und Fremdeinschätzung (r = {correlation:F2}).\n\n";
        
        text += $"Die inhaltliche Übereinstimmung beträgt {agreement:F1}%.\n\n";
        text += "Auswertung der Kompetenzen:\n";
        text += "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n";
        text += "Selbsteinschätzung:\n";
        for (var i = 0; i < 6; i++)
            text += $"  • {Norms.Competencies[i]}: {Norms.GetRatingText(seValues?[i] ?? 1)} ({(seValues?[i] ?? 1)}/5)\n";
        
        text += "\nFremdeinschätzung:\n";
        for (var i = 0; i < 6; i++)
            text += $"  • {Norms.Competencies[i]}: {Norms.GetRatingText(feValues?[i] ?? 1)} ({(feValues?[i] ?? 1)}/5)\n";
        
        return text;
    }
}