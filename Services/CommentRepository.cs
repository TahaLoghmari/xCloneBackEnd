using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public class CommentRepository : ICommentRepository
    {
        private readonly TwitterDbContext _context ; 
        private readonly ILikeRepository _like ; 
        private readonly IFollowRepository _follow ; 
        public CommentRepository( TwitterDbContext context , ILikeRepository like , IFollowRepository follow) 
        { 
            _context = context ; 
            _like = like ; 
            _follow = follow ;
        }
        public async Task<IEnumerable<CommentResponseDto?>> GetUserCommentsOnPost(int userId, int postId, int currentUserId)
        {
            var comments = await _context.Comments
                .AsNoTracking()
                .Include(c => c.Creator)
                .Where(c => c.UserId == userId && c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            if (comments == null || !comments.Any())
                return new List<CommentResponseDto?>();

            bool isFollowing = await _follow.IsUserFollowing(currentUserId, userId);
            
            return comments.Select(c => CommentResponseDto.Create(
                c,
                _like.HasLikedComment(currentUserId, c.Id),
                isFollowing
            )).ToList();
        }
        public async Task<IEnumerable<PostResponseDto?>> GetPostsWithUserComments(int userId, int currentUserId)
        {
            var postIds = await _context.Comments
                .Where(c => c.UserId == userId)
                .Select(c => c.PostId)
                .Distinct()
                .ToListAsync();
            var posts = await _context.Posts
                .AsNoTracking()
                .Include(p => p.Creator)
                .Include(p => p.OriginalPost)
                .ThenInclude(op => op != null ? op.Creator : null)
                .Where(p => postIds.Contains(p.Id))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var result = new List<PostResponseDto?>();
            foreach (var post in posts)
            {
                bool isLiked = _like.HasLikedPost(currentUserId, post.Id);
                bool isFollowing = await _follow.IsUserFollowing(currentUserId, post.UserId);
                result.Add(PostResponseDto.Create(post, isLiked, isFollowing));
            }

            return result;
        }
        public async Task<CommentResponseDto?> AddComment(CommentCreationDto newCommentDto , int userId , int postId )
        {
            var post = await _context.Posts.FirstOrDefaultAsync( p => p.Id == postId );
            if ( post == null ) return null ; 

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var newComment = new Comment {
                UserId = userId,
                PostId = postId,
                Content = newCommentDto.Content,
                LikesCount = 0,
                RepliesCount = 0,
            };
            
            post.CommentsCount += 1 ; 
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            var createdReply = await _context.Comments.Include(c => c.Creator).FirstOrDefaultAsync( c => c.Id == newComment.Id );
            
            return CommentResponseDto.Create(createdReply,_like.HasLikedComment(userId,createdReply.Id),false);
        }
        public async Task<CommentResponseDto?> ReplyToAComment(CommentCreationDto replyComment, int userId, int postId, int parentCommentId)
        {
            var parentComment = await _context.Comments.FindAsync(parentCommentId);
            if (parentComment == null) return null;

            var newComment = new Comment
            {
                UserId = userId,
                PostId = postId,
                Content = replyComment.Content,
                ParentCommentId = parentCommentId,
                LikesCount = 0,
                RepliesCount = 0,
            };

            parentComment.RepliesCount += 1;

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            var createdReply = await _context.Comments
                .AsNoTracking()
                .Include(c => c.Creator)
                .FirstOrDefaultAsync(c => c.Id == newComment.Id);

            return createdReply == null ? null : CommentResponseDto.Create(createdReply,_like.HasLikedComment(userId,createdReply.Id),false);
        }
        public async Task<CommentResponseDto?> GetCommentById(int commentId , int currentUserId )
        {
            var comment = await _context.Comments.Include(c => c.Creator).FirstOrDefaultAsync( c => c.Id == commentId );
            if ( comment == null ) return null ; 
            var followed = await _follow.IsUserFollowing(currentUserId,comment.Creator.Id) ; 
            return CommentResponseDto.Create(comment,_like.HasLikedComment(currentUserId,comment.Id),followed);
        }
        public async Task<IEnumerable<CommentResponseDto?>> GetCommentsForPost(int postId, int currentUserId)
        {
            var comments = await _context.Comments
                .AsNoTracking()
                .Include(c => c.Creator)
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
                
            var commentIds = comments.Select(c => c.Id).ToList();

            var likedCommentIds = await _context.Likes
                .Where(l => commentIds.Contains(l.CommentId.Value) && l.UserId == currentUserId)
                .Select(l => l.CommentId.Value)
                .ToListAsync();

            var creatorIds = comments.Select(c => c.Creator.Id).Distinct().ToList();
            var followedCreatorIds = await _context.Follows
                .Where(f => f.FollowerId == currentUserId && creatorIds.Contains(f.FollowingId))
                .Select(f => f.FollowingId)
                .ToListAsync();
            
            return comments.Select(c => CommentResponseDto.Create(
                c, 
                likedCommentIds.Contains(c.Id),
                followedCreatorIds.Contains(c.Creator.Id)
            )).ToList();
        }
        public async Task<IEnumerable<CommentResponseDto?>> GetRepliesForComment(int commentId, int currentUserId)
        {
            var replyComments = await _context.Comments
                .AsNoTracking()
                .Include(c => c.Creator)
                .Where(c => c.ParentCommentId == commentId)
                .ToListAsync();

            var commentIds = replyComments.Select(c => c.Id).ToList();

            var likedCommentIds = await _context.Likes
                .Where(l => commentIds.Contains(l.CommentId.Value) && l.UserId == currentUserId)
                .Select(l => l.CommentId.Value)
                .ToListAsync();

            var creatorIds = replyComments.Select(c => c.Creator.Id).Distinct().ToList();
            var followedCreatorIds = await _context.Follows
                .Where(f => f.FollowerId == currentUserId && creatorIds.Contains(f.FollowingId))
                .Select(f => f.FollowingId)
                .ToListAsync();
            
            return replyComments.Select(c => CommentResponseDto.Create(
                c, 
                likedCommentIds.Contains(c.Id),
                followedCreatorIds.Contains(c.Creator.Id)
            )).ToList();
        }
        public async Task<CommentResponseDto?> UpdateComment( CommentCreationDto updatedComment , int commentId , int currentUserId)
        {
            var oldComment = await _context.Comments.Include(c => c.Creator).FirstOrDefaultAsync( c => c.Id == commentId );
            if ( oldComment == null ) return null ;
            oldComment.Content = updatedComment.Content ; 
            oldComment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var following = await _follow.IsUserFollowing(currentUserId,oldComment.Creator.Id);
            return CommentResponseDto.Create(oldComment,_like.HasLikedComment(currentUserId,oldComment.Id),following);
        }
        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null) return false;
            
            if (comment.ParentCommentId.HasValue)
            {
                var parentComment = await _context.Comments.FindAsync(comment.ParentCommentId.Value);
                if (parentComment != null)
                    parentComment.RepliesCount--;
            }
            else 
            {
                var post = await _context.Posts.FindAsync(comment.PostId);
                if (post != null)
                    post.CommentsCount--;
            }
            
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}