using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;

[ApiController]
[Route("api/[controller]")]
public class CloudinaryController : ControllerBase
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryController(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }
    [HttpPost("get-signature")]
    public IActionResult GetSignature()
    {
        var timestamp = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
        var parameters = new System.Collections.Generic.Dictionary<string, object>
        {
            { "timestamp", timestamp },
            { "folder", "uploads" } 
        };

        var signature = _cloudinary.Api.SignParameters(parameters);
        return Ok(new { signature, timestamp });
    }
}