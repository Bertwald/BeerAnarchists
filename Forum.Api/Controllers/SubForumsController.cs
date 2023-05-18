using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Forum.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
[Authorize]
public class SubForumsController : ControllerBase
{
    private readonly ForumDbContext _context;
    private readonly UserManager<ForumUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SubForumsController(ForumDbContext context, UserManager<ForumUser> userManager, RoleManager<IdentityRole> roleManager) {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    // GET: api/SubForums
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubForum>>> GetSubforums()
    {
      if (_context.Subfora == null)
      {
          return NotFound();
      }
        return await _context.Subfora.ToListAsync();
    }

    // GET: api/SubForums/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SubForum>> GetSubForum(int id)
    {
      if (_context.Subfora == null)
      {
          return NotFound();
      }
        var subForum = await _context.Subfora.FindAsync(id);

        if (subForum == null)
        {
            return NotFound();
        }

        return subForum;
    }

    // PUT: api/SubForums/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSubForum(int id, SubForum subForum)
    {
        if (id != subForum.Id)
        {
            return BadRequest();
        }

        _context.Entry(subForum).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SubForumExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/SubForums
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<SubForum>> PostSubForum(SubForum subForum)
    {
      if (_context.Subfora == null)
      {
          return Problem("Entity set 'ForumDbContext.Subfora'  is null.");
      }
        _context.Subfora.Add(subForum);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetSubForum", new { id = subForum.Id }, subForum);
    }

    // DELETE: api/SubForums/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubForum(int id)
    {
        if (_context.Subfora == null)
        {
            return NotFound();
        }
        var subForum = await _context.Subfora.FindAsync(id);
        if (subForum == null)
        {
            return NotFound();
        }

        _context.Subfora.Remove(subForum);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SubForumExists(int id)
    {
        return (_context.Subfora?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
