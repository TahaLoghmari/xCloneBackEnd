using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public class LikeRepository : ILikeRepository
    {
        private readonly TwitterDbContext _context ; 
        public LikeRepository( TwitterDbContext context ) { _context = context ; }
        public async Task<LikeResponseDTO?> AddLikeToComment( int userId , int commentId )
        {
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

            return new LikeResponseDTO {
                Id = newLike.Id,
                UserId = newLike.UserId,
                CommentId = newLike.CommentId,
                Creator = new UserDto {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Username = user.UserName,
                    Email = user.Email,
                    ImageUrl = user.ImageUrl,
                    CreatedAt = user.CreatedAt,
                    FollowerCount = user.FollowerCount,
                    FollowingCount = user.FollowingCount,
                    BirthDate = user.BirthDate
                }
            };
        }
        public async Task<LikeResponseDTO?> AddLikeToPost( int userId , int postId )
        {
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

            return new LikeResponseDTO {
                Id = newLike.Id,
                UserId = newLike.UserId,
                PostId = newLike.PostId,
                Creator = new UserDto {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Username = user.UserName,
                    Email = user.Email,
                    ImageUrl = user.ImageUrl,
                    CreatedAt = user.CreatedAt,
                    FollowerCount = user.FollowerCount,
                    FollowingCount = user.FollowingCount,
                    BirthDate = user.BirthDate
                }
            };
        }

        public async Task<IEnumerable<LikeResponseDTO?>> GetLikesForComment(int commentId)
        {
            var Likes = await _context.Likes.Include( c => c.Creator )
            .Where( l => l.CommentId == commentId )
            .Select( c => new LikeResponseDTO {
                Id = c.Id,
                UserId = c.UserId,
                CommentId = c.CommentId,
                Creator = new UserDto {
                    Id = c.Creator.Id,
                    DisplayName = c.Creator.DisplayName,
                    Username = c.Creator.UserName,
                    Email = c.Creator.Email,
                    ImageUrl = c.Creator.ImageUrl,
                    CreatedAt = c.Creator.CreatedAt,
                    FollowerCount = c.Creator.FollowerCount,
                    FollowingCount = c.Creator.FollowingCount,
                    BirthDate = c.Creator.BirthDate
                }
            })
            .ToListAsync();
            return Likes ; 
        }

        public async Task<IEnumerable<LikeResponseDTO?>> GetLikesForPost(int postId)
        {
            var Likes = await _context.Likes.Include( c => c.Creator )
            .Where( l => l.PostId == postId )
            .Select( c => new LikeResponseDTO {
                Id = c.Id,
                UserId = c.UserId,
                PostId = c.PostId,
                Creator = new UserDto {
                    Id = c.Creator.Id,
                    DisplayName = c.Creator.DisplayName,
                    Username = c.Creator.UserName,
                    Email = c.Creator.Email,
                    ImageUrl = c.Creator.ImageUrl,
                    CreatedAt = c.Creator.CreatedAt,
                    FollowerCount = c.Creator.FollowerCount,
                    FollowingCount = c.Creator.FollowingCount,
                    BirthDate = c.Creator.BirthDate
                }
            })
            .ToListAsync();
            return Likes ; 
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
    }
}