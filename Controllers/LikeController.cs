using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBackEnd.Models.Dto;
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
        public async Task<ActionResult<LikeResponseDTO?>> AddLikeToComment( int userId , int commentId )
        {
            var newLike = await _like.AddLikeToComment( userId , commentId);
            if ( newLike == null ) return NotFound();
            return Ok(newLike);
        }
        [Authorize]
        [HttpPost("post/{postId}/{userId}")]
        public async Task<ActionResult<LikeResponseDTO>> AddLikeToPost( int userId , int postId )
        {
            var newLike = await _like.AddLikeToPost( userId ,  postId);
            if ( newLike == null ) return NotFound();
            return Ok(newLike);
        }
        [Authorize]
        [HttpGet("comment/{commentId}")]
        public async Task<ActionResult<IEnumerable<LikeResponseDTO?>>> GetLikesForComment( int commentId )
        {
            var Likes = await _like.GetLikesForComment(commentId);
            return Ok(Likes);
        }
        [Authorize]
        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<LikeResponseDTO?>>> GetLikesForPost(int postId )
        {
            var Likes = await _like.GetLikesForPost(postId);
            return Ok(Likes);
        }
        [Authorize]
        [HttpDelete("comment/{commentId}/{userId}")]
        public async Task<IActionResult> RemoveLikeFromComment( int userId , int commentId )
        {
            var removed = await _like.RemoveLikeFromComment( userId ,  commentId);
            if ( !removed ) return NotFound();
            return Ok("Like was successfuly removed from that comment."); 
        }
        [Authorize]
        [HttpDelete("post/{postId}/{userId}")]
        public async Task<IActionResult> RemoveLikeFromPost( int userId , int postId )
        {
            var removed = await _like.RemoveLikeFromPost( userId ,  postId);
            if ( !removed ) return NotFound();
            return Ok("Like was successfuly removed from that post."); 
        }
    }
}