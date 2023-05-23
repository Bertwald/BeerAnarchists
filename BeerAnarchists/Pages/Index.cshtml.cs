using Forum.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace BeerAnarchists.Pages;
public class IndexModel : PageModel {

    internal IEnumerable<SubForum> SubForums { get; set; }

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger) {
        _logger = logger;
    }

    public async void OnGet() {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7174/");
        client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        SubForums = await client.GetFromJsonAsync<IEnumerable<SubForum>>("/forum/SubFora") ?? Enumerable.Empty<SubForum>();
    }
}
