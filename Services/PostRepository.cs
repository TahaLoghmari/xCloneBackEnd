using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;


namespace TwitterCloneBackEnd.Services
{
    public class PostRepository : IPostRepository
    {
        private readonly TwitterDbContext _context ; 
        
        public PostRepository( TwitterDbContext context ) 
        {
            _context = context ; 
            
        }
        public async Task<IEnumerable<Post>> GetUserPosts(int UserId)
        {
            return await _context.Posts
                .Where(p => p.UserId == UserId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetPaginatedPosts(int page, int pageSize)
        {
            return await _context.Posts
                .Select(p => new Post
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    CommentsCount = p.CommentsCount,
                    SharesCount = p.SharesCount,
                    LikesCount = p.LikesCount,
                    ViewsCount = p.ViewsCount,
                    MediaUploadPath = p.MediaUploadPath,
                    MediaUploadType = p.MediaUploadType,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    OriginalPostId = p.OriginalPostId,
                    IsRetweet = p.IsRetweet,
                    Creator = new User
                    {
                        Id = p.Creator.Id,
                        UserName = p.Creator.UserName,
                        DisplayName = p.Creator.DisplayName,
                        ImageUrl = p.Creator.ImageUrl
                    }
                })
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Post> CreatePost(PostCreationDto newPostDto, int userId)
        {
            if (newPostDto == null)
            {
                throw new ArgumentNullException(nameof(newPostDto), "No file was uploaded");
            }

            var newPost = new Post
            {
                UserId = userId,
                CommentsCount = 0,
                SharesCount = 0,
                LikesCount = 0,
                ViewsCount = 0,
                Content = newPostDto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsRetweet = false,
                MediaUploadPath = newPostDto.MediaUploadPath,
                MediaUploadType = newPostDto.MediaUploadType
            };

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            
            return newPost;
        }
        public async Task<string> DeletePost(int postId)
        {
            var Post = await _context.Posts.FirstOrDefaultAsync( p => p.Id == postId );
            if ( Post == null ) throw new KeyNotFoundException($"Post with ID {postId} was not found.");
            _context.Posts.Remove(Post);
            await _context.SaveChangesAsync() ; 
            return "Post was sucessfully Deleted";
        }
        public async Task<Post> GetPostById(int postId)
        {
            var Post = await _context.Posts.FirstOrDefaultAsync( p => p.Id == postId );
            if ( Post == null ) throw new KeyNotFoundException($"Post with ID {postId} was not found.");
            return Post ; 
        }
        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            var allPosts = await _context.Posts
                .Select(p => new Post
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    CommentsCount = p.CommentsCount,
                    SharesCount = p.SharesCount,
                    LikesCount = p.LikesCount,
                    ViewsCount = p.ViewsCount,
                    MediaUploadPath = p.MediaUploadPath,
                    MediaUploadType = p.MediaUploadType,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    OriginalPostId = p.OriginalPostId,
                    IsRetweet = p.IsRetweet,
                    Creator = new User
                    {
                        Id = p.Creator.Id,
                        UserName = p.Creator.UserName,
                        DisplayName = p.Creator.DisplayName,
                        ImageUrl = p.Creator.ImageUrl
                    }
                })
                .ToListAsync();
            
            if (allPosts == null || !allPosts.Any()) 
                throw new KeyNotFoundException("No posts were found.");
                
            return allPosts;
        }
        public async Task<Post> UpdatePost(int postId, PostCreationDto updatedPost)
        {
            var Post = await _context.Posts.FirstOrDefaultAsync( p => p.Id == postId ) ;
            if ( Post == null ) throw new KeyNotFoundException($"Post with ID {postId} was not found.");
            Post.Content = updatedPost.Content ; 
            await _context.SaveChangesAsync();
            return Post ;
        }
        public async Task<Post> RetweetPost(int originalPostId, int userId )
        {
            var OriginalPost = await _context.Posts.FirstOrDefaultAsync( p => p.Id == originalPostId ) ;
            if (OriginalPost == null)
            {
                throw new KeyNotFoundException($"Original post with ID {originalPostId} was not found.");
            }
            var newPost = new Post
            {
                UserId = userId,
                CommentsCount = 0,
                SharesCount = 0,
                LikesCount = 0,
                ViewsCount = 0,
                Content = OriginalPost.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsRetweet = true,
                OriginalPostId = originalPostId ,
                MediaUploadPath = OriginalPost.MediaUploadPath,
                MediaUploadType = OriginalPost.MediaUploadType
            };
            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            
            return newPost;
        }
    }
}