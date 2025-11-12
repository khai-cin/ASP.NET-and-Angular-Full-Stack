

using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
   
    public class MembersController(AppDbContext context) : BaseAPIController
    {

        // async vs sync
        // async is non-blocking, a waitress can take other orders while waiting for your order to be prepared
        // sync is blocking, a waitress has to wait for your order to be prepared before taking other orders
        [HttpGet]
        public async  Task<ActionResult<IReadOnlyList<AppUser>>> GetMemeybers()
        {
            var members = await context.Users.ToListAsync();

            return members;
        }

        // [Authorize] means all endpoints in this controller require authorization
        [Authorize]
        [HttpGet("{id}")] // locaalhost:5001/api/members/jon-id
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var member = await context.Users.FindAsync(id);

            if (member == null) return NotFound();

            return member;
        }
    }
}