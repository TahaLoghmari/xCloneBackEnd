using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;
using TwitterCloneBackEnd.Services;

namespace TwitterCloneBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly IFollowRepository _follow ;
        public FollowController( IFollowRepository follow ) { _follow = follow ; } 

        [Authorize]
        [HttpPost("{followerId}/{followingId}")]
        public async Task<ActionResult<FollowResponseDTO?>> FollowUser(int followerId, int followingId)
        {
            if (followerId == followingId) return BadRequest("Users cannot follow themselves");
            var follow = await _follow.FollowUser(followerId, followingId);
            if (follow == null) return NotFound();
            return Ok(follow);
        }
        [Authorize]
        [HttpDelete("unfollow/{followerId}/{followingId}")]
        public async Task<IActionResult> UnFollowUser(int followerId, int followingId)
        {
            var success = await _follow.UnFollowUser(followerId, followingId);
            if (!success) return NotFound("Follow relationship not found");
            return Ok("Successfully unfollowed");
        }
        [Authorize]
        [HttpGet("{UserId}/{currentUserId}")]
        public async Task<ActionResult<IEnumerable<UserDto>?>> GetUserFollowers(int UserId , int currentUserId )
        {
            var Followers = await _follow.GetUserFollowers(UserId,currentUserId);
            return Ok(Followers);
        }
        [Authorize]
        [HttpGet("{UserId}/followings/{currentUserId}")]
        public async Task<ActionResult<IEnumerable<UserDto>?>> GetUserFollowings( int UserId , int currentUserId  )
        {
            var Followings = await _follow.GetUserFollowings(UserId,currentUserId); 
            return Ok(Followings);
        }
    }
}