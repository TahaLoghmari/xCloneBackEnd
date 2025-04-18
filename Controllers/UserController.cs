using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;
using TwitterCloneBackEnd.Services;


namespace TwitterCloneBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _User ; 
        public UserController( IUserRepository User )
        {
            _User = User ;
        }
        [Authorize]
        [HttpGet("{UserId}/{currentUserId}")]
        public async Task<ActionResult<UserDto>> GetUserProfile( int UserId , int currentUserId)
        {
            var User = await _User.GetUserProfile(UserId,currentUserId);
            if ( User == null ) 
            {
                return NotFound("User not found");
            }
            return Ok(User);
        }
        [Authorize]
        [HttpDelete("{UserId}")]
        public async Task<IActionResult> DeleteUserProfile( int UserId ) 
        {
            await _User.DeleteUserProfile(UserId);
            return Ok("User was Deleted successfuly");
        }
        [Authorize]
        [HttpPut("{UserId}/{currentUserId}")]
        public async Task<IActionResult> PutUserProfile( int UserId , [FromBody] UserEditDto UpdatedUser , int currentUserId)
        {
            await _User.PutUserProfile(UserId,UpdatedUser,currentUserId);
            return Ok("User was successfully Updated");
        }
    }
}