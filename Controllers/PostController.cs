using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;
using TwitterCloneBackEnd.Services;

namespace TwitterCloneBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _Post ; 
        public PostController( IPostRepository Post ) 
        {
            _Post = Post ; 
        }
        
        [Authorize]
        [HttpGet("{UserId}/posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetUserPosts( int UserId )
        {
            var Posts = await _Post.GetUserPosts(UserId) ; 
            if ( Posts == null ) 
            {
                return NotFound("No posts were found for this user");
            }
            return Ok(Posts) ; 
        }
        [Authorize]
        [HttpPost("addPost/{UserId}")]
        public async Task<ActionResult<Post>> CreatePost( [FromBody] PostCreationDto newPostDto , int UserId )
        {
            if ( newPostDto == null ) return BadRequest("No new Post Content");
            var newPost = await _Post.CreatePost(newPostDto,UserId);
            return Ok(newPost);
        }
        [Authorize]
        [HttpGet("paginatedPosts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPaginatedPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var paginatedPosts = await _Post.GetPaginatedPosts(page, pageSize);
            if (paginatedPosts == null) return NotFound("No Posts were Found");
            return Ok(paginatedPosts);
        }
        [Authorize]
        [HttpDelete("deletePost/{PostId}")]
        public async Task<IActionResult> DeletePost( int PostId ) 
        {
            var message = await _Post.DeletePost(PostId);
            return Ok(message);
        }
        [Authorize]
        [HttpGet("{PostId}")]
        public async Task<ActionResult<Post>> GetPostById( int PostId ) 
        {
            var post = await _Post.GetPostById(PostId);
            if ( post == null ) return NotFound("Post hasn't been found");
            return Ok(post);
        } 
        [Authorize]
        [HttpGet("allPosts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
        {
            var allPosts = await _Post.GetAllPosts();
            if ( allPosts == null ) return NotFound("No Posts were Found");
            return Ok(allPosts);
        }
        [Authorize]
        [HttpPut("updatePost/{PostId}")]
        public async Task<ActionResult<Post>> UpdatePost( int PostId , [FromBody] PostCreationDto updatedPost )
        {
            var newPost = await _Post.UpdatePost(PostId,updatedPost);
            return Ok(newPost);
        }
        [Authorize]
        [HttpPost("retweetPost/{originalPostId}/{UserId}")]
        public async Task<ActionResult<Post>> RetweetPost( int originalPostId , int UserId )
        {
            var retweetedPost = await _Post.RetweetPost(originalPostId,UserId);
            return Ok(retweetedPost);
        }
    }

}