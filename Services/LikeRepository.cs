using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;

namespace TwitterCloneBackEnd.Services
{
    public class LikeRepository : ILikeRepository
    {
        private readonly TwitterDbContext _context ; 
        public LikeRepository( TwitterDbContext context ) { _context = context ; }
        public async Task<Like> AddLikeToComment(int commentId, int userId)
        {
            var newLike = new Like {
                UserId = userId ,
                CommentId = commentId 
            };
            _context.Likes.Add(newLike);
            await _context.SaveChangesAsync();
            return newLike ;
        }

        public async Task<Like> AddLikeToPost(int postId, int userId)
        {
            var newLike = new Like {
                UserId = userId ,
                PostId = postId 
            };
            _context.Likes.Add(newLike);
            await _context.SaveChangesAsync();
            return newLike ;
        }

        public async Task<IEnumerable<Like>> GetLikesForComment(int commentId)
        {
            var Likes = await _context.Likes.Where( l => l.CommentId == commentId ).ToListAsync();
            return Likes ; 
        }

        public async Task<IEnumerable<Like>> GetLikesForPost(int postId)
        {
            var Likes = await _context.Likes.Where( l => l.PostId == postId ).ToListAsync();
            return Likes ; 
        }

        public async Task<bool> RemoveLikeFromComment(int commentId, int userId)
        {
            var Like = await _context.Likes.FirstOrDefaultAsync( l => l.CommentId == commentId && l.UserId == userId ) ; 
            if ( Like == null ) return false ;
            _context.Likes.Remove(Like);
            await _context.SaveChangesAsync();
            return true ;
        }

        public async Task<bool> RemoveLikeFromPost(int postId, int userId)
        {
            var Like = await _context.Likes.FirstOrDefaultAsync( l => l.PostId == postId && l.UserId == userId ) ; 
            if ( Like == null ) return false ;
            _context.Likes.Remove(Like);
            await _context.SaveChangesAsync();
            return true ;
        }
    }
}