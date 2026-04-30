using Newtonsoft.Json;

namespace DueskWPF.Models;

public class Profile
{
    [JsonProperty("profilID")]
    public string ProfilID { get; set; } = string.Empty;
    
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("gruppename")]
    public string? Gruppename { get; set; }
    
    [JsonProperty("gruppeID")]
    public string? GruppeID { get; set; }
    
    [JsonProperty("created_at")]
    public string? CreatedAt { get; set; }
    
    // 36 SE-Items
    [JsonProperty("item1")] public int Item1 { get; set; } = 2;
    [JsonProperty("item2")] public int Item2 { get; set; } = 2;
    [JsonProperty("item3")] public int Item3 { get; set; } = 2;
    [JsonProperty("item4")] public int Item4 { get; set; } = 2;
    [JsonProperty("item5")] public int Item5 { get; set; } = 2;
    [JsonProperty("item6")] public int Item6 { get; set; } = 2;
    [JsonProperty("item7")] public int Item7 { get; set; } = 2;
    [JsonProperty("item8")] public int Item8 { get; set; } = 2;
    [JsonProperty("item9")] public int Item9 { get; set; } = 2;
    [JsonProperty("item10")] public int Item10 { get; set; } = 2;
    [JsonProperty("item11")] public int Item11 { get; set; } = 2;
    [JsonProperty("item12")] public int Item12 { get; set; } = 2;
    [JsonProperty("item13")] public int Item13 { get; set; } = 2;
    [JsonProperty("item14")] public int Item14 { get; set; } = 2;
    [JsonProperty("item15")] public int Item15 { get; set; } = 2;
    [JsonProperty("item16")] public int Item16 { get; set; } = 2;
    [JsonProperty("item17")] public int Item17 { get; set; } = 2;
    [JsonProperty("item18")] public int Item18 { get; set; } = 2;
    [JsonProperty("item19")] public int Item19 { get; set; } = 2;
    [JsonProperty("item20")] public int Item20 { get; set; } = 2;
    [JsonProperty("item21")] public int Item21 { get; set; } = 2;
    [JsonProperty("item22")] public int Item22 { get; set; } = 2;
    [JsonProperty("item23")] public int Item23 { get; set; } = 2;
    [JsonProperty("item24")] public int Item24 { get; set; } = 2;
    [JsonProperty("item25")] public int Item25 { get; set; } = 2;
    [JsonProperty("item26")] public int Item26 { get; set; } = 2;
    [JsonProperty("item27")] public int Item27 { get; set; } = 2;
    [JsonProperty("item28")] public int Item28 { get; set; } = 2;
    [JsonProperty("item29")] public int Item29 { get; set; } = 2;
    [JsonProperty("item30")] public int Item30 { get; set; } = 2;
    [JsonProperty("item31")] public int Item31 { get; set; } = 2;
    [JsonProperty("item32")] public int Item32 { get; set; } = 2;
    [JsonProperty("item33")] public int Item33 { get; set; } = 2;
    [JsonProperty("item34")] public int Item34 { get; set; } = 2;
    [JsonProperty("item35")] public int Item35 { get; set; } = 2;
    [JsonProperty("item36")] public int Item36 { get; set; } = 2;
    
    // 36 FE-Items
    [JsonProperty("feitem1")] public int Feitem1 { get; set; } = 2;
    [JsonProperty("feitem2")] public int Feitem2 { get; set; } = 2;
    [JsonProperty("feitem3")] public int Feitem3 { get; set; } = 2;
    [JsonProperty("feitem4")] public int Feitem4 { get; set; } = 2;
    [JsonProperty("feitem5")] public int Feitem5 { get; set; } = 2;
    [JsonProperty("feitem6")] public int Feitem6 { get; set; } = 2;
    [JsonProperty("feitem7")] public int Feitem7 { get; set; } = 2;
    [JsonProperty("feitem8")] public int Feitem8 { get; set; } = 2;
    [JsonProperty("feitem9")] public int Feitem9 { get; set; } = 2;
    [JsonProperty("feitem10")] public int Feitem10 { get; set; } = 2;
    [JsonProperty("feitem11")] public int Feitem11 { get; set; } = 2;
    [JsonProperty("feitem12")] public int Feitem12 { get; set; } = 2;
    [JsonProperty("feitem13")] public int Feitem13 { get; set; } = 2;
    [JsonProperty("feitem14")] public int Feitem14 { get; set; } = 2;
    [JsonProperty("feitem15")] public int Feitem15 { get; set; } = 2;
    [JsonProperty("feitem16")] public int Feitem16 { get; set; } = 2;
    [JsonProperty("feitem17")] public int Feitem17 { get; set; } = 2;
    [JsonProperty("feitem18")] public int Feitem18 { get; set; } = 2;
    [JsonProperty("feitem19")] public int Feitem19 { get; set; } = 2;
    [JsonProperty("feitem20")] public int Feitem20 { get; set; } = 2;
    [JsonProperty("feitem21")] public int Feitem21 { get; set; } = 2;
    [JsonProperty("feitem22")] public int Feitem22 { get; set; } = 2;
    [JsonProperty("feitem23")] public int Feitem23 { get; set; } = 2;
    [JsonProperty("feitem24")] public int Feitem24 { get; set; } = 2;
    [JsonProperty("feitem25")] public int Feitem25 { get; set; } = 2;
    [JsonProperty("feitem26")] public int Feitem26 { get; set; } = 2;
    [JsonProperty("feitem27")] public int Feitem27 { get; set; } = 2;
    [JsonProperty("feitem28")] public int Feitem28 { get; set; } = 2;
    [JsonProperty("feitem29")] public int Feitem29 { get; set; } = 2;
    [JsonProperty("feitem30")] public int Feitem30 { get; set; } = 2;
    [JsonProperty("feitem31")] public int Feitem31 { get; set; } = 2;
    [JsonProperty("feitem32")] public int Feitem32 { get; set; } = 2;
    [JsonProperty("feitem33")] public int Feitem33 { get; set; } = 2;
    [JsonProperty("feitem34")] public int Feitem34 { get; set; } = 2;
    [JsonProperty("feitem35")] public int Feitem35 { get; set; } = 2;
    [JsonProperty("feitem36")] public int Feitem36 { get; set; } = 2;
    
    public int GetItem(int index) => index switch
    {
        1 => Item1, 2 => Item2, 3 => Item3, 4 => Item4, 5 => Item5,
        6 => Item6, 7 => Item7, 8 => Item8, 9 => Item9, 10 => Item10,
        11 => Item11, 12 => Item12, 13 => Item13, 14 => Item14, 15 => Item15,
        16 => Item16, 17 => Item17, 18 => Item18, 19 => Item19, 20 => Item20,
        21 => Item21, 22 => Item22, 23 => Item23, 24 => Item24, 25 => Item25,
        26 => Item26, 27 => Item27, 28 => Item28, 29 => Item29, 30 => Item30,
        31 => Item31, 32 => Item32, 33 => Item33, 34 => Item34, 35 => Item35,
        36 => Item36,
        _ => 2
    };
    
    public int GetFeItem(int index) => index switch
    {
        1 => Feitem1, 2 => Feitem2, 3 => Feitem3, 4 => Feitem4, 5 => Feitem5,
        6 => Feitem6, 7 => Feitem7, 8 => Feitem8, 9 => Feitem9, 10 => Feitem10,
        11 => Feitem11, 12 => Feitem12, 13 => Feitem13, 14 => Feitem14, 15 => Feitem15,
        16 => Feitem16, 17 => Feitem17, 18 => Feitem18, 19 => Feitem19, 20 => Feitem20,
        21 => Feitem21, 22 => Feitem22, 23 => Feitem23, 24 => Feitem24, 25 => Feitem25,
        26 => Feitem26, 27 => Feitem27, 28 => Feitem28, 29 => Feitem29, 30 => Feitem30,
        31 => Feitem31, 32 => Feitem32, 33 => Feitem33, 34 => Feitem34, 35 => Feitem35,
        36 => Feitem36,
        _ => 2
    };
    
    public List<int> GetAllSEItems()
    {
        var items = new List<int>();
        try
        {
            for (int i = 1; i <= 36; i++)
            {
                items.Add(GetItem(i));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetAllSEItems Error: {ex.Message}");
            return new List<int>(Enumerable.Repeat(2, 36));
        }
        return items;
    }
    
    public List<int> GetAllFEItems()
    {
        var items = new List<int>();
        try
        {
            for (int i = 1; i <= 36; i++)
            {
                items.Add(GetFeItem(i));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetAllFEItems Error: {ex.Message}");
            return new List<int>(Enumerable.Repeat(2, 36));
        }
        return items;
    }
}

public class ProfileCreate
{
    public string Name { get; set; } = string.Empty;
    public int? GruppeID { get; set; }
    public string? Gruppename { get; set; }
    
    public int Item1 { get; set; } = 2;
    public int Item2 { get; set; } = 2;
    public int Item3 { get; set; } = 2;
    public int Item4 { get; set; } = 2;
    public int Item5 { get; set; } = 2;
    public int Item6 { get; set; } = 2;
    public int Item7 { get; set; } = 2;
    public int Item8 { get; set; } = 2;
    public int Item9 { get; set; } = 2;
    public int Item10 { get; set; } = 2;
    public int Item11 { get; set; } = 2;
    public int Item12 { get; set; } = 2;
    public int Item13 { get; set; } = 2;
    public int Item14 { get; set; } = 2;
    public int Item15 { get; set; } = 2;
    public int Item16 { get; set; } = 2;
    public int Item17 { get; set; } = 2;
    public int Item18 { get; set; } = 2;
    public int Item19 { get; set; } = 2;
    public int Item20 { get; set; } = 2;
    public int Item21 { get; set; } = 2;
    public int Item22 { get; set; } = 2;
    public int Item23 { get; set; } = 2;
    public int Item24 { get; set; } = 2;
    public int Item25 { get; set; } = 2;
    public int Item26 { get; set; } = 2;
    public int Item27 { get; set; } = 2;
    public int Item28 { get; set; } = 2;
    public int Item29 { get; set; } = 2;
    public int Item30 { get; set; } = 2;
    public int Item31 { get; set; } = 2;
    public int Item32 { get; set; } = 2;
    public int Item33 { get; set; } = 2;
    public int Item34 { get; set; } = 2;
    public int Item35 { get; set; } = 2;
    public int Item36 { get; set; } = 2;
    
    public int Feitem1 { get; set; } = 2;
    public int Feitem2 { get; set; } = 2;
    public int Feitem3 { get; set; } = 2;
    public int Feitem4 { get; set; } = 2;
    public int Feitem5 { get; set; } = 2;
    public int Feitem6 { get; set; } = 2;
    public int Feitem7 { get; set; } = 2;
    public int Feitem8 { get; set; } = 2;
    public int Feitem9 { get; set; } = 2;
    public int Feitem10 { get; set; } = 2;
    public int Feitem11 { get; set; } = 2;
    public int Feitem12 { get; set; } = 2;
    public int Feitem13 { get; set; } = 2;
    public int Feitem14 { get; set; } = 2;
    public int Feitem15 { get; set; } = 2;
    public int Feitem16 { get; set; } = 2;
    public int Feitem17 { get; set; } = 2;
    public int Feitem18 { get; set; } = 2;
    public int Feitem19 { get; set; } = 2;
    public int Feitem20 { get; set; } = 2;
    public int Feitem21 { get; set; } = 2;
    public int Feitem22 { get; set; } = 2;
    public int Feitem23 { get; set; } = 2;
    public int Feitem24 { get; set; } = 2;
    public int Feitem25 { get; set; } = 2;
    public int Feitem26 { get; set; } = 2;
    public int Feitem27 { get; set; } = 2;
    public int Feitem28 { get; set; } = 2;
    public int Feitem29 { get; set; } = 2;
    public int Feitem30 { get; set; } = 2;
    public int Feitem31 { get; set; } = 2;
    public int Feitem32 { get; set; } = 2;
    public int Feitem33 { get; set; } = 2;
    public int Feitem34 { get; set; } = 2;
    public int Feitem35 { get; set; } = 2;
    public int Feitem36 { get; set; } = 2;
}