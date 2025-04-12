using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public interface ICommentRepository 
    {

        Task<Comment> AddComment(CommentCreationDto newComment , int userId , int postId );
        Task<Comment> ReplyToAComment(CommentCreationDto replyComment , int userId , int postId , int parentCommentId );
        Task<IEnumerable<Comment>> GetPaginatedComments(int page, int pageSize , int postId );
        Task<Comment?> GetCommentById(int commentId);
        Task<IEnumerable<Comment>> GetCommentsForPost(int postId);
        Task<IEnumerable<Comment>> GetRepliesForComment(int commentId);

        Task<Comment?> UpdateComment(CommentCreationDto updatedComment,int commentId);

        Task<bool> DeleteComment(int commentId);
    }
}