using TwitterCloneBackEnd.Models;

namespace TwitterCloneBackEnd.Services
{
    public interface ILikeRepository
    {
        Task<Like> AddLikeToPost(int postId, int userId);
        Task<Like> AddLikeToComment(int commentId, int userId);
        Task<bool> RemoveLikeFromPost(int postId, int userId);
        Task<bool> RemoveLikeFromComment(int commentId, int userId);
        Task<IEnumerable<Like>> GetLikesForPost(int postId);
        Task<IEnumerable<Like>> GetLikesForComment(int commentId);
    }
}