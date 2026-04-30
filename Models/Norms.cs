namespace DueskWPF.Models;

public static class Norms
{
    // HS Normen (Hauptschule)
    public static readonly double[][] NormSE_HS = new[]
    {
        new[] { 21.33, 25.33, 29.33, 33.32, 37.32 },
        new[] { 20.87, 24.95, 29.03, 33.13, 37.18 },
        new[] { 17.93, 21.37, 24.80, 28.23, 31.67 },
        new[] { 13.98, 17.71, 21.44, 25.17, 28.90 },
        new[] { 24.60, 28.55, 33.04, 37.53, 42.01 },
        new[] { 15.53, 18.97, 22.40, 25.83, 29.27 }
    };
    
    public static readonly double[][] NormFE_HS = new[]
    {
        new[] { 12.66, 18.16, 23.66, 29.16, 34.66 },
        new[] { 13.33, 18.42, 23.51, 28.60, 33.69 },
        new[] { 10.75, 15.41, 20.07, 24.73, 29.39 },
        new[] { 14.22, 15.30, 16.38, 17.46, 18.54 },
        new[] { 14.12, 20.21, 26.30, 32.39, 38.48 },
        new[] { 10.53, 14.51, 18.49, 22.47, 26.45 }
    };
    
    // FS Normen (Förderschule)
    public static readonly double[][] NormSE_FS = new[]
    {
        new[] { 17.54, 24.03, 30.53, 37.02, 43.51 },
        new[] { 17.80, 24.26, 30.73, 37.19, 43.65 },
        new[] { 18.03, 22.41, 26.79, 31.17, 35.55 },
        new[] { 14.28, 15.55, 16.83, 18.10, 19.37 },
        new[] { 20.69, 27.49, 34.29, 41.09, 47.89 },
        new[] { 12.44, 18.06, 23.68, 29.29, 34.91 }
    };
    
    public static readonly double[][] NormFE_FS = new[]
    {
        new[] { 15.30, 19.79, 24.28, 28.77, 33.26 },
        new[] { 14.63, 18.94, 23.25, 27.56, 31.87 },
        new[] { 14.62, 17.81, 21.00, 24.19, 27.38 },
        new[] { 15.00, 15.55, 16.10, 16.65, 17.20 },
        new[] { 18.44, 22.61, 26.78, 30.95, 35.12 },
        new[] { 9.79, 13.97, 18.15, 22.33, 26.51 }
    };
    
    public static readonly string[] Competencies = 
    {
        "Arbeitsverhalten", "Lernverhalten", "Sozialverhalten",
        "Fachkompetenz", "Personale Kompetenz", "Methodenkompetenz"
    };
    
    public static readonly string[] Items = 
    {
        "Zuverlässigkeit", "Arbeitstempo", "Arbeitsplanung", "Organisationsfähigkeit",
        "Geschicklichkeit", "Ordnung", "Sorgfalt", "Kreativität", "Problemlösungsfähigkeit",
        "Abstraktionsvermögen", "Selbstständigkeit", "Belastbarkeit", "Konzentrationsfähigkeit",
        "Verantwortungsbewusstsein", "Eigeninitiative", "Leistungsbereitschaft", "Auffassungsgabe",
        "Merkfähigkeit", "Motivationsfähigkeit", "Reflektionsfähigkeit", "Teamfähigkeit",
        "Hilfsbereitschaft", "Kontaktfähigkeit", "Respektvoller Umgang", "Kommunikationsfähigkeit",
        "Einfühlungsvermögen", "Konfliktfähigkeit", "Kritikfähigkeit", "Schreiben", "Lesen",
        "Mathematik", "Naturwissenschaft", "Fremdsprachen", "Präsentationsfähigkeit",
        "PC Kenntnisse", "Fächerübergreifendes Denken"
    };
    
    public static string GetRatingText(int value) => value switch
    {
        1 => "weit unterdurchschnittlich",
        2 => "unterdurchschnittlich",
        3 => "durchschnittlich",
        4 => "überdurchschnittlich",
        5 => "weit überdurchschnittlich",
        _ => "unbekannt"
    };
}