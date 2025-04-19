using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;
using TwitterCloneBackEnd.Services;

namespace TwitterCloneBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _comment ; 
        public CommentController( ICommentRepository comment ) { _comment = comment ; }
        [Authorize]
        [HttpPost("{userId}/{postId}")]
        public async Task<ActionResult<CommentResponseDto?>> AddComment( [FromBody] CommentCreationDto newCommentDto , int userId , int postId )
        {
            Console.WriteLine("right here bro " , postId);
            var newComment = await _comment.AddComment(newCommentDto,userId,postId);
            if ( newComment == null ) return BadRequest();
            return Ok(newComment);
        }
        [Authorize]
        [HttpPost("{userId}/{postId}/{parentCommentId}")]
        public async Task<ActionResult<CommentResponseDto?>> ReplyToAComment( [FromBody] CommentCreationDto replyComment , int userId , int postId , int parentCommentId)
        {
            var newComment = await _comment.ReplyToAComment(replyComment,userId,postId,parentCommentId);
            if ( newComment == null ) return BadRequest();
            return Ok(newComment);
        }
        [Authorize]
        [HttpGet("{commentId}/{currentUserId}")]
        public async Task<ActionResult<CommentResponseDto?>> GetCommentById( int commentId , int currentUserId)
        {
            var comment = await _comment.GetCommentById(commentId,currentUserId);
            if ( comment == null ) return NotFound();
            return Ok(comment);
        }
        [Authorize]
        [HttpGet("user/{userId}/post/{postId}/{currentUserId}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto?>>> GetUserCommentsOnPost(int userId, int postId, int currentUserId)
        {
            var comments = await _comment.GetUserCommentsOnPost(userId, postId, currentUserId);
            return Ok(comments);
        }
        [Authorize]
        [HttpGet("user/{userId}/commented-posts/{currentUserId}")]
        public async Task<ActionResult<IEnumerable<PostResponseDto?>>> GetPostsWithUserComments(int userId, int currentUserId)
        {
            var posts = await _comment.GetPostsWithUserComments(userId, currentUserId);
            if (posts == null || !posts.Any()) return NotFound("No posts with comments were found for this user");
            return Ok(posts);
        }
        [Authorize]
        [HttpGet("post/{postId}/{currentUserId}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto?>>> GetCommentsForPost( int postId , int currentUserId) 
        {
            var comments = await _comment.GetCommentsForPost(postId,currentUserId);
            return Ok(comments);
        }
        [Authorize]
        [HttpGet("replies/{commentId}/{currentUserId}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto?>>> GetRepliesForComment( int commentId , int currentUserId)
        {
            var comments = await _comment.GetRepliesForComment(commentId,currentUserId);
            return Ok(comments);
        }
        [Authorize]
        [HttpPut("{commentId}/{currentUserId}")]
        public async Task<ActionResult<CommentResponseDto?>> UpdateComment( [FromBody] CommentCreationDto updatedComment , int commentId , int currentUserId) 
        {
            var newComment = await _comment.UpdateComment(updatedComment,commentId,currentUserId);
            if ( newComment == null ) return BadRequest();
            return Ok(newComment);
        }
        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment( int commentId ) 
        {
            var response = await _comment.DeleteComment(commentId);
            if ( !response ) return NotFound() ; 
            return Ok("Comment was successfuly deleted");
        }
    }
}