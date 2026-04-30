using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DueskWPF.Models;

namespace DueskWPF.Services;

public class ApiService
{
    private static readonly HttpClient _httpClient = new();
    private const string BaseUrl = "https://paul-koop.org/api/";
    
    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        var data = new { username, password };
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{BaseUrl}api_login.php", content);
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<LoginResponse>(json) ?? new LoginResponse();
    }
    
    public async Task<List<Profile>> GetProfilesAsync()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}api_profiles.php");
        foreach (var header in SessionManager.Instance.GetAuthHeaders())
            request.Headers.Add(header.Key, header.Value);
        
        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Profile>>(json) ?? new List<Profile>();
    }
    
    public async Task<Profile?> GetProfileAsync(string id)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}api_profiles.php?id={id}");
        foreach (var header in SessionManager.Instance.GetAuthHeaders())
            request.Headers.Add(header.Key, header.Value);
        
        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Profile>(json);
    }
    
    public async Task<bool> CreateProfileAsync(ProfileCreate profile)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}api_profiles.php");
        foreach (var header in SessionManager.Instance.GetAuthHeaders())
            request.Headers.Add(header.Key, header.Value);
        
        request.Content = new StringContent(JsonConvert.SerializeObject(profile), Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> UpdateProfileAsync(Profile profile)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}api_profiles.php");
        foreach (var header in SessionManager.Instance.GetAuthHeaders())
            request.Headers.Add(header.Key, header.Value);
        
        request.Headers.Add("X-Profile-ID", profile.ProfilID);
        request.Content = new StringContent(JsonConvert.SerializeObject(profile), Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> DeleteProfileAsync(string id)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}api_profiles.php?id={id}");
        foreach (var header in SessionManager.Instance.GetAuthHeaders())
            request.Headers.Add(header.Key, header.Value);
        
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<List<Group>> GetGroupsAsync()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}api_groups.php");
        foreach (var header in SessionManager.Instance.GetAuthHeaders())
            request.Headers.Add(header.Key, header.Value);
        
        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Group>>(json) ?? new List<Group>();
    }
    
    public async Task<bool> CreateGroupAsync(string name)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}api_groups.php");
        foreach (var header in SessionManager.Instance.GetAuthHeaders())
            request.Headers.Add(header.Key, header.Value);
        
        var data = new { name };
        request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> DeleteGroupAsync(int id)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}api_groups.php?id={id}");
        foreach (var header in SessionManager.Instance.GetAuthHeaders())
            request.Headers.Add(header.Key, header.Value);
        
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
}