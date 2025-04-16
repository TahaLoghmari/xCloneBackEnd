using TwitterCloneBackEnd.Models.Dto ;

namespace TwitterCloneBackEnd.Services
{
    public interface ILikeRepository
    {
        Task<LikeResponseDTO?> AddLikeToPost( int userId , int postId );
        Task<LikeResponseDTO?> AddLikeToComment( int userId , int commentId );
        Task<bool> RemoveLikeFromPost( int userId , int postId);
        Task<bool> RemoveLikeFromComment( int userId , int commentId );
        Task<IEnumerable<LikeResponseDTO?>> GetLikesForPost(int postId);
        Task<IEnumerable<LikeResponseDTO?>> GetLikesForComment(int commentId);
    }
}