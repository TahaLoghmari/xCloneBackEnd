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

            return new CommentResponseDto {
                Id = newComment.Id ,
                UserId = newComment.UserId,
                PostId = newComment.PostId ,
                ParentCommentId = newComment.ParentCommentId ,
                Content = newComment.Content,
                CreatedAt = newComment.CreatedAt ,
                LikesCount = newComment.LikesCount,
                RepliesCount = newComment.RepliesCount,
                Creator = new UserDto {
                    Id = user.Id,
                    Username = user.UserName,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    ImageUrl = user.ImageUrl,
                    CreatedAt = user.CreatedAt,
                    FollowerCount = user.FollowerCount,
                    FollowingCount = user.FollowingCount,
                    BirthDate = user.BirthDate
                }
            };
        }
        public async Task<CommentResponseDto?> ReplyToAComment(CommentCreationDto replyComment , int userId , int postId , int parentCommentId)
        {
            var parentComment = await _context.Comments.FirstOrDefaultAsync( c => c.Id == parentCommentId );
            if ( parentComment == null ) return null ; 

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var newComment = new Comment {
                UserId = userId,
                PostId = postId,
                Content = replyComment.Content,
                ParentCommentId = parentCommentId ,
                LikesCount = 0,
                RepliesCount = 0,
            };
            
            parentComment.RepliesCount += 1 ; 
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return new CommentResponseDto {
                Id = newComment.Id ,
                UserId = newComment.UserId,
                PostId = newComment.PostId ,
                ParentCommentId = newComment.ParentCommentId ,
                Content = newComment.Content,
                CreatedAt = newComment.CreatedAt ,
                LikesCount = newComment.LikesCount,
                RepliesCount = newComment.RepliesCount,
                Creator = new UserDto {
                    Id = user.Id,
                    Username = user.UserName,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    ImageUrl = user.ImageUrl,
                    CreatedAt = user.CreatedAt,
                    FollowerCount = user.FollowerCount,
                    FollowingCount = user.FollowingCount,
                    BirthDate = user.BirthDate
                }
            };
        }
        public async Task<CommentResponseDto?> GetCommentById(int commentId)
        {
            var comment = await _context.Comments.Include(c => c.Creator).FirstOrDefaultAsync( c => c.Id == commentId );
            if ( comment == null ) return null ; 


            return new CommentResponseDto {
                Id = comment.Id ,
                UserId = comment.UserId,
                PostId = comment.PostId ,
                ParentCommentId = comment.ParentCommentId ,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt ,
                LikesCount = comment.LikesCount,
                RepliesCount = comment.RepliesCount,
                Creator = new UserDto {
                    Id = comment.Creator.Id,
                    Username = comment.Creator.UserName,
                    DisplayName = comment.Creator.DisplayName,
                    Email = comment.Creator.Email,
                    ImageUrl = comment.Creator.ImageUrl,
                    CreatedAt = comment.Creator.CreatedAt,
                    FollowerCount = comment.Creator.FollowerCount,
                    FollowingCount = comment.Creator.FollowingCount,
                    BirthDate = comment.Creator.BirthDate
                }
            };
        }
        public async Task<IEnumerable<CommentResponseDto?>> GetCommentsForPost(int postId)
        {
            var comments = await _context.Comments
                .Include(c => c.Creator)
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .Select(c => new CommentResponseDto {
                    Id = c.Id,
                    UserId = c.UserId,
                    PostId = c.PostId,
                    ParentCommentId = c.ParentCommentId,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    LikesCount = c.LikesCount,
                    RepliesCount = c.RepliesCount,
                    Creator = new UserDto {
                        Id = c.UserId,
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
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return comments;
        }
        public async Task<IEnumerable<CommentResponseDto?>> GetRepliesForComment(int commentId)
        {
            var replyComments = await _context.Comments
            .Include(c => c.Creator)
            .Where( c => c.ParentCommentId == commentId )
            .Select(c => new CommentResponseDto {
                Id = c.Id ,
                UserId = c.UserId,
                PostId = c.PostId ,
                ParentCommentId = c.ParentCommentId ,
                Content = c.Content,
                CreatedAt = c.CreatedAt ,
                LikesCount = c.LikesCount,
                RepliesCount = c.RepliesCount,
                Creator = new UserDto {
                    Id = c.UserId,
                    DisplayName = c.Creator.DisplayName,
                    Username = c.Creator.UserName,
                    Email = c.Creator.Email,
                    ImageUrl = c.Creator.ImageUrl,
                    CreatedAt = c.Creator.CreatedAt,
                    FollowerCount = c.Creator.FollowerCount,
                    FollowingCount = c.Creator.FollowingCount,
                    BirthDate = c.Creator.BirthDate
                }
            }).ToListAsync();
            return replyComments;
        }
        public async Task<CommentResponseDto?> UpdateComment( CommentCreationDto updatedComment , int commentId )
        {
            var oldComment = await _context.Comments.Include(c => c.Creator).FirstOrDefaultAsync( c => c.Id == commentId );
            if ( oldComment == null ) return null ;
            oldComment.Content = updatedComment.Content ; 
            oldComment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new CommentResponseDto {
                Id = oldComment.Id ,
                UserId = oldComment.UserId,
                PostId = oldComment.PostId ,
                ParentCommentId = oldComment.ParentCommentId ,
                Content = oldComment.Content,
                CreatedAt = oldComment.CreatedAt ,
                LikesCount = oldComment.LikesCount,
                RepliesCount = oldComment.RepliesCount,
                Creator = new UserDto {
                    Id = oldComment.UserId,
                    Username = oldComment.Creator.UserName,
                    DisplayName = oldComment.Creator.DisplayName,
                    Email = oldComment.Creator.Email,
                    ImageUrl = oldComment.Creator.ImageUrl,
                    CreatedAt = oldComment.Creator.CreatedAt,
                    FollowerCount = oldComment.Creator.FollowerCount,
                    FollowingCount = oldComment.Creator.FollowingCount,
                    BirthDate = oldComment.Creator.BirthDate
                }
            };
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