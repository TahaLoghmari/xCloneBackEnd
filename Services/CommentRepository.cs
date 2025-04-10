using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public class CommentRepository : ICommentRepository
    {
        private readonly TwitterDbContext _context ; 
        public CommentRepository( TwitterDbContext context ) { _context = context ; }

        public async Task<Comment> AddComment(CommentCreationDto newCommentDto , int userId , int postId )
        {
            var newComment = new Comment {
                UserId = userId,
                PostId = postId,
                Content = newCommentDto.Content,
                LikesCount = 0,
                RepliesCount = 0,
            };
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();
            return newComment ;
        }
        public async Task<Comment> ReplyToAComment(CommentCreationDto replyComment , int userId , int postId , int parentCommentId)
        {
            var newComment = new Comment {
                UserId = userId,
                PostId = postId,
                Content = replyComment.Content,
                ParentCommentId = parentCommentId ,
                LikesCount = 0,
                RepliesCount = 0,
            };
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();
            return newComment ;
        }
        public Task<Comment?> GetCommentById(int commentId)
        {
            var comment = _context.Comments.FirstOrDefaultAsync( c => c.Id == commentId );
            return comment ; 
        }
        public async Task<IEnumerable<Comment>> GetCommentsForPost(int postId)
        {
            var comments = await _context.Comments.Where( c => c.PostId == postId ).ToListAsync();
            return comments ;
        }
        public async Task<IEnumerable<Comment>> GetRepliesForComment(int commentId)
        {
            var replyComments = await _context.Comments.Where( c => c.ParentCommentId == commentId ).ToListAsync();
            return replyComments;
        }
        public async Task<Comment?> UpdateComment( CommentCreationDto updatedComment , int commentId )
        {
            var oldComment = await _context.Comments.FirstOrDefaultAsync( c => c.Id == commentId );
            if ( oldComment == null ) return null ;
            oldComment.Content = updatedComment.Content ; 
            await _context.SaveChangesAsync();
            return oldComment ;
        }
        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync( c => c.Id == commentId );
            if ( comment == null ) return false ; 
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true ; 
        }
    }
}