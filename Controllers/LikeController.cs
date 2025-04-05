using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Services;

namespace TwitterCloneBackEnd.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LikeController : ControllerBase 
    {
        private readonly ILikeRepository _like ; 
        public LikeController( ILikeRepository like ) { _like = like ;}

        [Authorize]
        [HttpPost("comment/{commentId}/{userId}")]
        public async Task<ActionResult<Like>> AddLikeToComment( int commentId , int userId )
        {
            var newLike = await _like.AddLikeToComment(commentId,userId);
            if ( newLike == null ) return NotFound();
            return Ok(newLike);
        }
        [Authorize]
        [HttpPost("post/{postId}/{userId}")]
        public async Task<ActionResult<Like>> AddLikeToPost( int postId , int userId )
        {
            var newLike = await _like.AddLikeToPost(postId,userId);
            if ( newLike == null ) return NotFound();
            return Ok(newLike);
        }
        [Authorize]
        [HttpGet("comment/{commentId}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikesForComment(int commentId )
        {
            var Likes = await _like.GetLikesForComment(commentId);
            if ( !Likes.Any()) return NotFound();
            return Ok(Likes);
        }
        [Authorize]
        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikesForPost(int postId )
        {
            var Likes = await _like.GetLikesForPost(postId);
            if ( !Likes.Any() ) return NotFound();
            return Ok(Likes);
        }
        [Authorize]
        [HttpDelete("comment/{commentId}/{userId}")]
        public async Task<IActionResult> RemoveLikeFromComment( int commentId , int userId )
        {
            var removed = await _like.RemoveLikeFromComment(commentId,userId);
            if ( !removed ) return NotFound();
            return Ok("Like was successfuly removed from that comment."); 
        }
        [Authorize]
        [HttpDelete("post/{postId}/{userId}")]
        public async Task<IActionResult> RemoveLikeFromPost( int postId , int userId )
        {
            var removed = await _like.RemoveLikeFromPost(postId,userId);
            if ( !removed ) return NotFound();
            return Ok("Like was successfuly removed from that post."); 
        }
    }
}