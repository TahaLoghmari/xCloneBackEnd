using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBackEnd.Models;
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
        public async Task<ActionResult<Follow>> FollowUser( int followerId , int followingId )
        {
            var follow = await _follow.FollowUser( followerId , followingId ) ;
            if ( follow == null ) return NotFound() ;
            return Ok(follow);
        }
        [Authorize]
        [HttpDelete("unfollow/{followerId}/{followingId}")]
        public async Task<IActionResult> UnFollowUser( int followerId , int followingId )
        {
            var message = await _follow.UnFollowUser( followerId , followingId ) ;
            return Ok(message);
        }
        [Authorize]
        [HttpGet("{UserId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserFollowers( int UserId )
        {
            var Followers = await _follow.GetUserFollowers(UserId);
            if ( !Followers.Any() )
            {
                return NotFound("No followers were found for this user ");
            }
            return Ok(Followers);
        }
        [Authorize]
        [HttpGet("{UserId}/followings")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserFollowings( int UserId )
        {
            var Followings = await _follow.GetUserFollowings(UserId);
            if ( !Followings.Any() )
            {
                return NotFound("No followings were found for this user");
            }
            return Ok(Followings);
        }
    }
}