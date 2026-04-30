using Newtonsoft.Json;

namespace DueskWPF.Models;

public class LoginResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }
    
    [JsonProperty("userID")]
    public string? UserID { get; set; }
    
    [JsonProperty("session")]
    public string? Session { get; set; }
    
    [JsonProperty("error")]
    public string? Error { get; set; }
}