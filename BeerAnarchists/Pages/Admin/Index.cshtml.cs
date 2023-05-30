using Forum.Data.Models;
using Forum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BeerAnarchists.Pages.RoleAdmin;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    public List<ForumUser> Users { get; set; }
    public List<IdentityRole> URoles { get; set; }

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
    private readonly JwtTokenService _jwtTokenService;
    private readonly AdminService _adminService;
    //private readonly TestController _testController;

    private Task<IEnumerable<Forum.Data.Models.SubForum>> _subForums;


    public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<ForumUser> userManager, JwtTokenService jwt, AdminService adm)
    {
        _roleManager= roleManager;
        _userManager= userManager;
        _jwtTokenService= jwt;
        _adminService= adm;
    }

    public async Task<IEnumerable<Forum.Data.Models.SubForum>> GetSubforums() {
        IEnumerable<Forum.Data.Models.SubForum> result = Enumerable.Empty<Forum.Data.Models.SubForum>();
        using (var client = new HttpClient()) {
            client.BaseAddress = new Uri("https://localhost:7174/");
            HttpResponseMessage response = await client.GetAsync("api/SubForums");
            if (response.IsSuccessStatusCode) {
                string responseString = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<List<Forum.Data.Models.SubForum>>(responseString);
            }
        }
        return result;
    }

    public async void PostSubforum(string name = "default") {
        using (var client = new HttpClient()) {
            var currentUser = await _userManager.GetUserAsync(User);
            //var username = currentUser.Claims.Where(x => x.Type == "preferred-username").Select(x => x.Value).FirstOrDefault();
            Forum.Data.Models.SubForum subForum = new Forum.Data.Models.SubForum() { Author = currentUser?.Alias ?? currentUser?.UserName ?? "PlaceHolder", Title = name };
            var json = JsonSerializer.Serialize(subForum);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("https://localhost:7174");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtTokenService.CreateToken(currentUser));
            //HttpResponseMessage response = await client.PostAsync("api/SubForums/", httpContent);
            HttpResponseMessage response = await client.GetAsync("api/SubForums/");
            if (response.IsSuccessStatusCode) {

                //string responseString = await response.Content.ReadAsStringAsync();
                //result = JsonSerializer.Deserialize<List<SubForum>>(responseString);
            }
        }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        URoles = await _roleManager.Roles.ToListAsync();
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

        //PostSubforum("First Forum");
        //_subForums = GetSubforums();
        var client2 = new HttpClient();
        client2.BaseAddress = new Uri("https://localhost:7208/");
        client2.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        var b = await client2.GetFromJsonAsync<IEnumerable<ForumUser>>("Test");
        /*
        var a = await client2.GetAsync("Test");
        if (a.IsSuccessStatusCode) {
            var products = a.Content.ReadAsAsync<ForumUser>();
        }
        */
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
