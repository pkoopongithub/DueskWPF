using System;
using System.IO;
using System.Text.Json;

namespace DueskWPF.Services;

public class SessionManager
{
    private static readonly Lazy<SessionManager> _instance = new(() => new SessionManager());
    public static SessionManager Instance => _instance.Value;
    
    private const string SessionFile = "session.json";
    
    public string? UserId { get; private set; }
    public string? Session { get; private set; }
    public bool IsLoggedIn => !string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(Session);
    
    private SessionManager() { }
    
    public void SetSession(string userId, string session)
    {
        UserId = userId;
        Session = session;
        SaveSession();
    }
    
    public void Clear()
    {
        UserId = null;
        Session = null;
        if (File.Exists(SessionFile))
            File.Delete(SessionFile);
    }
    
    public void SaveSession()
    {
        if (!IsLoggedIn) return;
        
        var data = new { UserId, Session };
        var json = JsonSerializer.Serialize(data);
        File.WriteAllText(SessionFile, json);
    }
    
    public void LoadSession()
    {
        if (!File.Exists(SessionFile)) return;
        
        try
        {
            var json = File.ReadAllText(SessionFile);
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (data != null)
            {
                UserId = data.GetValueOrDefault("UserId");
                Session = data.GetValueOrDefault("Session");
            }
        }
        catch { /* Ignore errors */ }
    }
    
    public Dictionary<string, string> GetAuthHeaders()
    {
        return new Dictionary<string, string>
        {
            ["X-User-ID"] = UserId ?? string.Empty,
            ["X-Session"] = Session ?? string.Empty
        };
    }
}