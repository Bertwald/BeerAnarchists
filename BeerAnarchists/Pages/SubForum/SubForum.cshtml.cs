using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.SubForum;

public class SubForumModel : PageModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Author { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<ForumThread> ForumThreads { get; set; }


    private readonly ISubforum _subforum;

    public SubForumModel(ISubforum subforum)
    {
        _subforum = subforum;
    }

    public void OnGet(int id)
    {
        var subForum = _subforum.GetById(id);
        if (subForum != null)
        {
            Id = id;
            Title = subForum.Title;
            Description = subForum.Description ?? "No description yet";
            Author = subForum.Author;
            ImageUrl = subForum.ImageUrl;
            ForumThreads = subForum.ForumThreads;
        }
    }

    /*
    internal class ExpandedThread : ForumThread {
        public ForumUser LastPoster { get; set; }
        public DateTime LastPost { get; set; }
    }
    */

}