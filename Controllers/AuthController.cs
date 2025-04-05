using TwitterCloneBackEnd.Models.Dto;
using TwitterCloneBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TwitterCloneBackEnd.Models.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TwitterDbContext _context ; 
    private readonly IConfiguration _configuration;
    private readonly TokenService _tokenService;
    public AuthController( TwitterDbContext context , IConfiguration configuration , TokenService tokenService) 
    {
        _context = context ; 
        _configuration = configuration;
        _tokenService = tokenService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] loginDto user)
    {
        var userFromDb = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == user.Email);
        
        if (userFromDb != null && BCrypt.Net.BCrypt.Verify(user.Password, userFromDb.PasswordHash))
        {
            var token = _tokenService.GenerateJwtToken(userFromDb.Username,userFromDb.Id);
            return Ok(new { token });
        }
        
        return Unauthorized();
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (await _context.Users.AnyAsync(u => u.Username == model.Username))
        {
            return BadRequest("Username already exists");
        }
        if ( await _context.Users.AnyAsync( u => u.Email.ToLower() == model.Email.ToLower() ) )
        {
            return BadRequest("Email already exists");
        }
        var user = new User
        {
            Username = model.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            Email = model.Email , 
            BirthDate = model.BirthDate,
            ImageUrl = model.ImageUrl
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var token = _tokenService.GenerateJwtToken(user.Username,user.Id);
        return Ok(new { token });
    }
}