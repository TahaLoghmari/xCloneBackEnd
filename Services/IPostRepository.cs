using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public interface IPostRepository 
    {
        Task<IEnumerable<Post>> GetUserPosts(int UserId );
        Task<Post> CreatePost(PostCreationDto newPost , int UserId );
        Task<string> DeletePost(int postId);
        Task<Post> GetPostById(int postId);
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post> UpdatePost(int postId, PostCreationDto updatedPost);
        Task<Post> RetweetPost(int originalPostId, int userId );
        Task<IEnumerable<Post>> GetPaginatedPosts(int page, int pageSize);
    }
}