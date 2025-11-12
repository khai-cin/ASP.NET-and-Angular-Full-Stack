

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interface;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        // Create claims for the token
        // Exception are used to handle errors that may occur during token creation
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot get token key");
        if (tokenKey.Length < 64)
        {
            throw new Exception("Token key must be at least 64 characters");
        }

        // SymmetricSecurityKey is used to create a security key from the token key
        // key is used to sign the token, ensuring its integrity and authenticity
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        // Create a list of claims to be included in the token
        // Claims are pieces of information about the user that are encoded in the token
        var claims = new List<Claim>
        {
            // Add claims for the user's display name, email, and unique identifier
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.NameIdentifier, user.Id)
        };

        // Create signing credentials using the security key and HMACSHA512 algorithm
        // creds will be used to sign the token
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // Create the token descriptor, which contains information about the token
        // It includes the subject (claims), expiration time, and signing credentials
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Set the subject of the token to the claims identity, which contains the user's claims
            Subject = new ClaimsIdentity(claims),
            // Set the token to expire in 7 days
            Expires = DateTime.UtcNow.AddDays(7),
            // Set the signing credentials to the previously created creds
            SigningCredentials = creds
        };

        // Create the token using the token handler and the token descriptor, return the serialized token string
        // tokenHandler is responsible for creating and writing the token
        var tokenHandler = new JwtSecurityTokenHandler();
        // Create the token, which represents the JWT
        // toeken is created based on the information in the token descriptor
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Return the serialized token string
        return tokenHandler.WriteToken(token);
    } 
}