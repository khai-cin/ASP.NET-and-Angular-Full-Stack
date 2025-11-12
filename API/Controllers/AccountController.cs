
using System.Security.Cryptography;
using System.Text;
using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseAPIController
{
    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
    {
        // using statement ensures that the HMACSHA512 object is disposed of properly after use
        using var hmac = new HMACSHA512();

        if (await EmailExists(registerDTO.Email)) return BadRequest("Error: Email is already in use");

        // var means variable, the type is inferred by the compiler
        // Create a new AppUser object and set its properties
        var user = new AppUser
        {
            DisplayName = registerDTO.DisplayName,
            Email = registerDTO.Email,
            // Compute the password hash using HMACSHA512 and set the PasswordHash and PasswordSalt properties
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
            PasswordSalt = hmac.Key
        };

        // Add the new user to the database context and save changes asynchronously
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToDTO(tokenService);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == loginDTO.Email);
        if (user == null) return Unauthorized("Invalid email");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }

        return user.ToDTO(tokenService);
    }

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}