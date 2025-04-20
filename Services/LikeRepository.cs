using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public class LikeRepository : ILikeRepository
    {
        private readonly TwitterDbContext _context ; 
        private readonly IFollowRepository _follow;
        private readonly INotificationRepository _notificationRepository;
        public LikeRepository( TwitterDbContext context, IFollowRepository follow , INotificationRepository notificationRepository) 
        { 
            _context = context ; 
            _follow = follow ; 
            _notificationRepository = notificationRepository;
        }
        public async Task<LikeResponseDTO?> AddLikeToComment( int userId , int commentId )
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId);
            if (existingLike != null) return null; 

            var user = await _context.Users.FirstOrDefaultAsync( u => u.Id == userId );
            if ( user == null ) return null ;

            var comment = await _context.Comments.FirstOrDefaultAsync( c => c.Id == commentId );
            if ( comment == null ) return null ; 

            var newLike = new Like {
                UserId = userId ,
                CommentId = commentId 
            };

            comment.LikesCount += 1 ; 

            _context.Likes.Add(newLike);
            await _context.SaveChangesAsync();

            if (userId != comment.UserId)
            {
                await _notificationRepository.CreateNotification(
                    creatorUserId: userId,
                    receiverUserId: comment.UserId,
                    type: NotificationType.Like,
                    commentId: commentId
                );
            }

            var Like = await _context.Likes.Include( l => l.Creator ).FirstOrDefaultAsync( l => l.Id == newLike.Id );

            bool isFollowing = await _follow.IsUserFollowing(userId, Like.UserId);
            return LikeResponseDTO.Create(Like, isFollowing);
        }
        public async Task<LikeResponseDTO?> AddLikeToPost( int userId , int postId )
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            if (existingLike != null) return null; 

            var user = await _context.Users.FirstOrDefaultAsync( u => u.Id == userId );
            if ( user == null ) return null ;

            var post = await _context.Posts.FirstOrDefaultAsync( c => c.Id == postId );
            if ( post == null ) return null ; 

            var newLike = new Like {
                UserId = userId ,
                PostId = postId 
            };

            post.LikesCount += 1 ;

            _context.Likes.Add(newLike);
            await _context.SaveChangesAsync();

            if (userId != post.UserId)
            {
                await _notificationRepository.CreateNotification(
                    creatorUserId: userId,
                    receiverUserId: post.UserId,
                    type: NotificationType.Like,
                    postId: postId
                );
            }

            var Like = await _context.Likes.Include( l => l.Creator ).FirstOrDefaultAsync( l => l.Id == newLike.Id );

            bool isFollowing = await _follow.IsUserFollowing(userId, Like.UserId);
            return LikeResponseDTO.Create(Like, isFollowing);
        }
        public async Task<IEnumerable<LikeResponseDTO?>> GetLikesForComment(int commentId, int currentUserId)
        {
            var likes = await _context.Likes.Include(c => c.Creator)
                .Where(l => l.CommentId == commentId)
                .ToListAsync();
                
            var results = new List<LikeResponseDTO>();
            foreach (var like in likes)
            {
                bool isFollowing = await _follow.IsUserFollowing(currentUserId, like.UserId);
                var dto = LikeResponseDTO.Create(like, isFollowing);
                if (dto != null)
                    results.Add(dto);
            }
            
            return results;
        }

       public async Task<IEnumerable<LikeResponseDTO?>> GetLikesForPost(int postId, int currentUserId)
        {
            var likes = await _context.Likes.Include(c => c.Creator)
                .Where(l => l.PostId == postId)
                .ToListAsync();
                
            var results = new List<LikeResponseDTO>();
            foreach (var like in likes)
            {
                bool isFollowing = await _follow.IsUserFollowing(currentUserId, like.UserId);
                var dto = LikeResponseDTO.Create(like, isFollowing);
                if (dto != null)
                    results.Add(dto);
            }
            
            return results;
        }

        public async Task<bool> RemoveLikeFromComment( int userId , int commentId )
        {
            var Like = await _context.Likes.FirstOrDefaultAsync( l => l.CommentId == commentId && l.UserId == userId ) ; 
            if ( Like == null ) return false ;

            var comment = await _context.Comments.FindAsync(commentId);
            if ( comment == null ) return false ; 

            comment.LikesCount -= 1 ; 

            _context.Likes.Remove(Like);
            await _context.SaveChangesAsync();

            return true ;
        }

        public async Task<bool> RemoveLikeFromPost( int userId , int postId )
        {
            var Like = await _context.Likes.FirstOrDefaultAsync( l => l.PostId == postId && l.UserId == userId ) ; 
            if ( Like == null ) return false ;

            var Post = await _context.Posts.FirstOrDefaultAsync( p => p.Id == postId );
            if ( Post == null ) return false ; 

            Post.LikesCount -= 1 ; 
            
            _context.Likes.Remove(Like);
            await _context.SaveChangesAsync();
            return true ;
        }
        public bool HasLikedPost(int userId, int postId)
        {
            return _context.Likes.Any(l => l.PostId == postId && l.UserId == userId);
        }
        public bool HasLikedComment(int userId, int commentId)
        {
            return _context.Likes.Any(l => l.CommentId == commentId && l.UserId == userId);
        }
    }
}