using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public interface IPostRepository 
    {
        Task<IEnumerable<PostResponseDto?>> GetUserPosts(int UserId , int currentUserId);
        Task<PostResponseDto?> CreatePost(PostCreationDto newPost , int UserId );
        Task<bool> DeletePost(int postId);
        Task<PostResponseDto?> GetPostById(int postId, int currentUserId);
        Task<IEnumerable<PostResponseDto?>> GetAllPosts(int currentUserId);
        Task<PostResponseDto?> UpdatePost(int postId, PostCreationDto updatedPost , int currentUserId);
        Task<PostResponseDto?> RetweetPost(int originalPostId, int userId );
        Task<IEnumerable<PostResponseDto?>> GetFollowingsPosts(int userId);
    }
}