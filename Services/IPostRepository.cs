using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public interface IPostRepository 
    {
        Task<IEnumerable<PostResponseDto?>> GetUserPosts(int UserId );
        Task<PostResponseDto?> CreatePost(PostCreationDto newPost , int UserId );
        Task<bool> DeletePost(int postId);
        Task<PostResponseDto?> GetPostById(int postId);
        Task<IEnumerable<PostResponseDto?>> GetAllPosts();
        Task<PostResponseDto?> UpdatePost(int postId, PostCreationDto updatedPost);
        Task<PostResponseDto?> RetweetPost(int originalPostId, int userId );
    }
}