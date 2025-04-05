using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;

[ApiController]
[Route("api/[controller]")]
public class OAuthController : ControllerBase
{
    private readonly TwitterDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly TokenService _tokenService;

    public OAuthController(TwitterDbContext context, IConfiguration configuration , TokenService tokenService)
    {
        _context = context;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    [HttpGet("challenge/{provider}")]
    public IActionResult Challenge(string provider, string returnUrl = null)
    {

        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(HandleCallback), 
                            values: new { returnUrl, provider }),
            Items = { { "scheme", provider } }
        };
        
        return Challenge(props, provider);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> HandleCallback(string returnUrl = null, string provider = null)
    {

        var info = await HttpContext.AuthenticateAsync(provider);
        if (!info.Succeeded) return Unauthorized();
        
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email.Split('@')[0] ;
        var externalId = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
        {
            if (await _context.Users.AnyAsync(u => u.Username == name))
            {
                name = $"{name}{new Random().Next(1000, 9999)}";
            }
            user = new User
            {
                Username = name , 
                Email = email,
                ExternalProvider = provider,
                ExternalId = externalId,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()), 
                ImageUrl = info.Principal.FindFirstValue("picture") ?? 
                            info.Principal.FindFirstValue("urn:google:picture") ?? "", 
                BirthDate = DateTime.UtcNow, 
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        

        var token = _tokenService.GenerateJwtToken(user.Username, user.Id);
        

        var clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL") ?? "http://localhost:5173";
        return Redirect($"{clientUrl}/auth/callback?token={token}");
    }
}