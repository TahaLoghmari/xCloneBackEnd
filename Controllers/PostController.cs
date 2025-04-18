using System.Security.Claims;
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
        [HttpGet("{UserId}/posts/{currentUserId}")]
        public async Task<ActionResult<IEnumerable<PostResponseDto?>>> GetUserPosts( int UserId , int currentUserId )
        {
            var Posts = await _Post.GetUserPosts(UserId,currentUserId) ; 
            if ( Posts == null ) return NotFound("No posts were found for this user");
            return Ok(Posts) ; 
        }
        [Authorize]
        [HttpPost("addPost/{UserId}")]
        public async Task<ActionResult<PostResponseDto?>> CreatePost( [FromBody] PostCreationDto newPostDto , int UserId )
        {
            if ( newPostDto == null ) return BadRequest("No new Post Content");
            var newPost = await _Post.CreatePost(newPostDto,UserId);
            return Ok(newPost);
        }
        [Authorize]
        [HttpDelete("deletePost/{PostId}")]
        public async Task<IActionResult> DeletePost( int PostId ) 
        {
            var message = await _Post.DeletePost(PostId);
            if ( !message ) return BadRequest();
            return Ok();
        }
        [Authorize]
        [HttpGet("{PostId}/{currentUserId}")]
        public async Task<ActionResult<PostResponseDto?>> GetPostById( int PostId , int currentUserId) 
        {
            var post = await _Post.GetPostById(PostId,currentUserId);
            if ( post == null ) return NotFound("Post hasn't been found");
            return Ok(post);
        } 
        [Authorize]
        [HttpGet("allPosts/{currentUserId}")]
        public async Task<ActionResult<IEnumerable<PostResponseDto?>>> GetAllPosts( int currentUserId)
        {
            Console.WriteLine("CurrentUserId");
            Console.WriteLine(currentUserId);
            var allPosts = await _Post.GetAllPosts(currentUserId);
            if ( allPosts == null ) return NotFound("No Posts were Found");
            return Ok(allPosts);
        }
        [Authorize]
        [HttpPut("updatePost/{PostId}/{currentUserId}")]
        public async Task<ActionResult<PostResponseDto?>> UpdatePost( int PostId , [FromBody] PostCreationDto updatedPost , int currentUserId)
        {
            var newPost = await _Post.UpdatePost(PostId,updatedPost,currentUserId);
            return Ok(newPost);
        }
        [Authorize]
        [HttpPost("retweetPost/{originalPostId}/{UserId}")]
        public async Task<ActionResult<PostResponseDto?>> RetweetPost( int originalPostId , int UserId )
        {
            var retweetedPost = await _Post.RetweetPost(originalPostId,UserId);
            return Ok(retweetedPost);
        }
    }

}