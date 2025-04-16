using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public interface ICommentRepository 
    {

        Task<CommentResponseDto?> AddComment(CommentCreationDto newComment , int userId , int postId );
        Task<CommentResponseDto?> ReplyToAComment(CommentCreationDto replyComment , int userId , int postId , int parentCommentId );
        Task<CommentResponseDto?> GetCommentById(int commentId);
        Task<IEnumerable<CommentResponseDto?>> GetCommentsForPost(int postId);
        Task<IEnumerable<CommentResponseDto?>> GetRepliesForComment(int commentId);

        Task<CommentResponseDto?> UpdateComment(CommentCreationDto updatedComment,int commentId);

        Task<bool> DeleteComment(int commentId);
    }
}