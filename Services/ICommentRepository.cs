using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public interface ICommentRepository 
    {

        Task<CommentResponseDto?> AddComment(CommentCreationDto newComment , int userId , int postId );
        Task<CommentResponseDto?> ReplyToAComment(CommentCreationDto replyComment , int userId , int postId , int parentCommentId );
        Task<CommentResponseDto?> GetCommentById(int commentId , int currentUserId);
        Task<IEnumerable<CommentResponseDto?>> GetCommentsForPost(int postId , int currentUserId);
        Task<IEnumerable<CommentResponseDto?>> GetRepliesForComment(int commentId , int currentUserId);

        Task<CommentResponseDto?> UpdateComment(CommentCreationDto updatedComment,int commentId , int currentUserId);
        Task<IEnumerable<PostResponseDto?>> GetPostsWithUserComments(int userId, int currentUserId);
        Task<IEnumerable<CommentResponseDto?>> GetUserCommentsOnPost(int userId, int postId, int currentUserId);

        Task<bool> DeleteComment(int commentId);
    }
}