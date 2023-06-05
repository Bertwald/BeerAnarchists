using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages;
public class AddReactionModel : PageModel {

    private readonly IForumPost _postService;
    private readonly UserManager<ForumUser> _userManager;

    public AddReactionModel(IForumPost postService, UserManager<ForumUser> userManager) {
        _postService = postService;
        _userManager = userManager;
    }

    public async Task<ActionResult> OnGet(int threadId, string userId, int postId, ReactionType type) {
        await AddReaction(userId, postId, type);

        return RedirectToPage("/Thread/Thread", new { id = threadId });
    }

    public async Task AddReaction(string userId, int postId, ReactionType reaction) {
        var checkPost = _postService.GetForumPostById(postId);
        var reactions = checkPost?.Reactions.ToList();
        //Check if there is already an reaction of this type from this user, we dont want multiple likes
        //Also check that [preferred pronoun] can not like [preferred pronoun]s own posts 
        var forbidden = reactions?.Where(x => x.Type == reaction && x.User.Id == userId && x.Post.Author.Id == userId).ToList();
        if (forbidden?.Count < 1 && checkPost?.Author.Id != userId) {
            var newReaction = new Reaction() {
                User = await _userManager.FindByIdAsync(userId),
                Type = reaction,
                Post = checkPost,
            };
            await _postService.AddReaction(postId, newReaction);
        }
        return;
    }
}
