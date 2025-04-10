using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TwitterDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(TwitterDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto user)
    {
        var userFromDb = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == user.Email);

        if (userFromDb != null && BCrypt.Net.BCrypt.Verify(user.Password, userFromDb.PasswordHash))
        {
            var token = GenerateJwtToken(userFromDb.UserName, userFromDb.Id);
            return Ok(new { token });
        }

        return Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (await _context.Users.AnyAsync(u => u.Email == model.Email))
        {
            return BadRequest("Email already exists");
        }
        if (await _context.Users.AnyAsync(u => u.UserName == model.UserName))
        {
            return BadRequest("UserName already exists");
        }
        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email,
            ImageUrl = model.ImageUrl,
            DisplayName = model.DisplayName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            BirthDate = model.BirthDate
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var token = GenerateJwtToken(user.UserName, user.Id);
        return Ok(new { token });
    }
    private string GenerateJwtToken(string username, int userId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
        _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiry = DateTime.Now.AddMinutes(
            double.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
