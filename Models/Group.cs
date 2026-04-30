using Newtonsoft.Json;

namespace DueskWPF.Models;

public class Group
{
    [JsonProperty("gruppeID")]
    public int GruppeID { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}