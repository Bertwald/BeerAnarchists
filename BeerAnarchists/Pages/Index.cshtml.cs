﻿using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace BeerAnarchists.Pages;
public class IndexModel : PageModel {

    private readonly ISubforum _subForumService;
    private UserManager<ForumUser> _userManager;

    // Model used to show data on page
    internal IEnumerable<SubForumModel> SubForums { get; set; } = Enumerable.Empty<SubForumModel>();

    public IndexModel(UserManager<ForumUser> userManager, ISubforum subforumService) {
        _userManager = userManager;
        _subForumService = subforumService;
    }

    public async Task OnGet() {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7174/");
        client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        var currentUser = await _userManager.GetUserAsync(User);
        SubForums = _subForumService.GetSubforums().Select(x => new SubForumModel {
            Id = x.Id,
            Author = x.Author,
            Description = x.Description,
            ImageUrl = x.ImageUrl,
            Title = x.Title
        });
        
        
        
        //if (currentUser != null) {
        //    var test = await client.GetFromJsonAsync<ForumUser>($"/forum/UserByMail/{currentUser?.Email}");
        //    var result = await client.DeleteAsync($"/forum/DeleteThread/1");
        //}



    }

    public class SubForumModel {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? ImageUrl { get; set; }
    }
}
