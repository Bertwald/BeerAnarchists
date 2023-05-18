using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Forum.Data.Models;
using System.Text.Json;

namespace BeerAnarchists.Pages.RoleAdmin;

//[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    public List<ForumUser> Users { get; set; }
    public List<IdentityRole> Roles { get; set; }

    [BindProperty]
    public string RoleName { get; set; }



    [BindProperty(SupportsGet = true)]
    public string AddUserId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string RemoveUserId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Role { get; set; }


    public bool IsUser { get; set; }
    public bool IsAdmin { get; set; }


    public readonly UserManager<ForumUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    private Task<IEnumerable<SubForum>> _subForums;


    public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<ForumUser> userManager)
    {
        _roleManager= roleManager;
        _userManager= userManager;
    }

    public async Task<IEnumerable<SubForum>> GetSubforums() {
        IEnumerable<SubForum> result = Enumerable.Empty<SubForum>();
        using (var client = new HttpClient()) {
            client.BaseAddress = new Uri("https://localhost:7174/");
            HttpResponseMessage response = await client.GetAsync("api/SubForums");
            if (response.IsSuccessStatusCode) {
                string responseString = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<List<SubForum>>(responseString);
            }
        }
        return result;
    }

    public async void PostSubforum(string name = "default") {
        using (var client = new HttpClient()) {
            var currentUser = await _userManager.GetUserAsync(User);
            SubForum subForum = new SubForum() { Author = currentUser.UserName, Title = name };
            var json = JsonSerializer.Serialize(subForum);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("https://localhost:7174/");
            HttpResponseMessage response = await client.PostAsync("api/SubForums/", httpContent);
            if (response.IsSuccessStatusCode) {

                //string responseString = await response.Content.ReadAsStringAsync();
                //result = JsonSerializer.Deserialize<List<SubForum>>(responseString);
            }
        }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Roles = await _roleManager.Roles.ToListAsync();
        Users = await _userManager.Users.ToListAsync();

        if(AddUserId != null)
        {
            var alterUser = await _userManager.FindByIdAsync(AddUserId);
            var roleresult = await _userManager.AddToRoleAsync(alterUser, Role);
        }

        if(RemoveUserId != null)
        {
            var alterUser = await _userManager.FindByIdAsync(RemoveUserId);
            var roleresult = await _userManager.RemoveFromRoleAsync(alterUser, Role);
        }

        // Demo av roller

        
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser != null)
        {
            IsUser = await _userManager.IsInRoleAsync(currentUser, "User");
            IsAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
        }

        PostSubforum("First Forum");
        _subForums = GetSubforums();

        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if(RoleName != null)
        {
            await CreateRole(RoleName);
        }
        return RedirectToPage("./Index");
    }

    public async Task CreateRole(string roleName)
    {
        bool exist = await _roleManager.RoleExistsAsync(roleName);
        if(!exist)
        {
            var Role = new IdentityRole
            {
                Name = roleName
            };

            await _roleManager.CreateAsync(Role);
        }
    }

}
