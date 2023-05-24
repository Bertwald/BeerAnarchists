using Forum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace BeerAnarchists.Pages;
public class IndexModel : PageModel {

    internal IEnumerable<SubForum> SubForums { get; set; } = Enumerable.Empty<SubForum>();
    internal UserManager<ForumUser> _userManager;

    public IndexModel(UserManager<ForumUser> userManager) {
        _userManager = userManager;
    }

    public async Task OnGet() {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7174/");
        client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        var currentUser = await _userManager.GetUserAsync(User);
        SubForums = await client.GetFromJsonAsync<IEnumerable<SubForum>>("/forum/SubFora") ?? Enumerable.Empty<SubForum>();
        if (currentUser != null) {
            var test = await client.GetFromJsonAsync<ForumUser>($"/forum/User/{currentUser?.Email}");
        }
    }
}
