using Forum.Data.Models;
using System.Text.Json;

namespace Forum.Services;
public class ApiService {
    private Uri _baseAdress = new Uri("https://localhost:7174/");


    public async Task<int> GetNumberOfPosts() {
        int result = 0;
        using var client = new HttpClient();
        client.BaseAddress = _baseAdress;
        HttpResponseMessage response = await client.GetAsync("/forum/TotalPosts");
        if (response.IsSuccessStatusCode) {
            string responseString = await response.Content.ReadAsStringAsync();
            result = JsonSerializer.Deserialize<int>(responseString);
        }
        return result;
    }

    public async Task<IEnumerable<ForumUser>> GetTopPosters() {
        List<ForumUser> posters = new();
        using var client = new HttpClient();
        client.BaseAddress = _baseAdress;
        HttpResponseMessage response = await client.GetAsync("/forum/TopPosters");
        if (response.IsSuccessStatusCode) {
            string responseString = await response.Content.ReadAsStringAsync();
            posters = JsonSerializer.Deserialize<List<Forum.Data.Models.ForumUser>>(responseString);
        }
        return posters;
    }

    public async Task<int> GetRegisteredUsers() {
        int result = 0;
        using var client = new HttpClient();
        client.BaseAddress = _baseAdress;
        HttpResponseMessage response = await client.GetAsync("/forum/RegisteredUsers");
        if (response.IsSuccessStatusCode) {
            string responseString = await response.Content.ReadAsStringAsync();
            result = JsonSerializer.Deserialize<int>(responseString);
        }
        return result;
    }

}
