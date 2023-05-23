using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace BeerAnarchists.Controllers;
[ApiController]
[Authorize(Roles = "Admin")]
[Route("[controller]")]
public class TestController : ControllerBase {

    private ForumDbContext _context;

    public TestController(ForumDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ForumUser>>> GetForumThreads() {
        if (_context.Users == null) {
            return NotFound();
        }
        return await _context.Users.ToListAsync();
    }


    /*
    // GET: api/Test
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public ActionResult<ForumUser> Index() {
        var currentUser = GetCurrentUser();
        return currentUser;
    }

    private ForumUser GetCurrentUser() {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null) {
            var userClaims = identity.Claims;
            return new ForumUser {
                UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                Alias = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
            };
        }
        return null;
    }
    */

}
