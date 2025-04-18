using TwitterCloneBackEnd.Models.Dto ;

namespace TwitterCloneBackEnd.Services
{
    public interface ILikeRepository
    {
        Task<LikeResponseDTO?> AddLikeToPost( int userId , int postId );
        Task<LikeResponseDTO?> AddLikeToComment( int userId , int commentId );
        Task<bool> RemoveLikeFromPost( int userId , int postId);
        Task<bool> RemoveLikeFromComment( int userId , int commentId );
        Task<IEnumerable<LikeResponseDTO?>> GetLikesForPost(int postId , int currentUserId);
        Task<IEnumerable<LikeResponseDTO?>> GetLikesForComment(int commentId , int currentUserId);
        public bool HasLikedPost(int userId, int postId);
        bool HasLikedComment(int userId, int commentId);
    }
}