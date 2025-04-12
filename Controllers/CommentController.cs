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
        public async Task<ActionResult<Comment>> AddComment( [FromBody] CommentCreationDto newCommentDto , int userId , int postId )
        {
            var newComment = await _comment.AddComment(newCommentDto,userId,postId);
            if ( newComment == null ) return BadRequest();
            return Ok(newComment);
        }
        [Authorize]
        [HttpPost("{userId}/{postId}/{parentCommentId}")]
        public async Task<ActionResult<Comment>> ReplyToAComment( [FromBody] CommentCreationDto replyComment , int userId , int postId , int parentCommentId)
        {
            var newComment = await _comment.ReplyToAComment(replyComment,userId,postId,parentCommentId);
            if ( newComment == null ) return BadRequest();
            return Ok(newComment);
        }
        [Authorize]
        [HttpGet("{commentId}")]
        public async Task<ActionResult<Comment>> GetCommentById( int commentId )
        {
            var comment = await _comment.GetCommentById(commentId);
            if ( comment == null ) return NotFound();
            return Ok(comment);
        }
        [Authorize]
        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsForPost( int postId ) 
        {
            var comments = await _comment.GetCommentsForPost(postId);
            if ( !comments.Any() ) return NotFound();
            return Ok(comments);
        }
        [Authorize]
        [HttpGet("post/paginated/{postId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetPaginatedComments(int postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var comments = await _comment.GetPaginatedComments(page,pageSize,postId);
            return Ok(comments);
        }
        [Authorize]
        [HttpGet("replies/{commentId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetRepliesForComment( int commentId )
        {
            var comments = await _comment.GetRepliesForComment(commentId);
            if ( !comments.Any() ) return NotFound();
            return Ok(comments);
        }
        [Authorize]
        [HttpPut("{commentId}")]
        public async Task<ActionResult<Comment>> UpdateComment( [FromBody] CommentCreationDto updatedComment , int commentId ) 
        {
            var newComment = await _comment.UpdateComment(updatedComment,commentId);
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