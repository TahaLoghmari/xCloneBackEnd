using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;


namespace TwitterCloneBackEnd.Services
{
    public class PostRepository : IPostRepository
    {
        private readonly TwitterDbContext _context ; 
        private readonly ILikeRepository _like ; 
        private readonly IFollowRepository _follow;
        
        public PostRepository( TwitterDbContext context , ILikeRepository like , IFollowRepository follow) 
        {
            _context = context ; 
            _like = like ; 
            _follow = follow;
        }
        public async Task<bool> DeletePost(int postId)
        {
            var Post = await _context.Posts.FirstOrDefaultAsync( p => p.Id == postId );
            if ( Post == null ) return false ;

            _context.Posts.Remove(Post);
            await _context.SaveChangesAsync() ; 

            return true ;
        }
        public async Task<IEnumerable<PostResponseDto?>> GetUserPosts(int UserId, int currentUserId)
        {
            var posts = await _context.Posts
                .Include(p => p.Creator)
                .Include(p => p.OriginalPost)
                .ThenInclude(op => op != null ? op.Creator : null)
                .Where(p => p.UserId == UserId)
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

        public async Task<PostResponseDto?> CreatePost(PostCreationDto newPostDto, int userId)
        {
            if (newPostDto == null) return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

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
            
            var createdPost = await _context.Posts
                .Include(p => p.Creator)
                .FirstAsync(p => p.Id == newPost.Id);

            bool isLiked = _like.HasLikedPost(userId, createdPost.Id);
            bool isFollowing = await _follow.IsUserFollowing(userId, createdPost.UserId);
            return PostResponseDto.Create(createdPost, isLiked, isFollowing);
        }

        public async Task<PostResponseDto?> GetPostById(int postId, int currentUserId)
        {
            var Post = await _context.Posts
                .Include(p => p.Creator)
                .Include(p => p.OriginalPost)
                .ThenInclude(op => op.Creator)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (Post == null) return null;

            bool isLiked = _like.HasLikedPost(currentUserId, Post.Id);
            bool isFollowing = await _follow.IsUserFollowing(currentUserId, Post.UserId);
            return PostResponseDto.Create(Post, isLiked, isFollowing);
        }

        public async Task<IEnumerable<PostResponseDto?>> GetAllPosts(int currentUserId)
        {
            var posts = await _context.Posts
                .AsNoTracking()
                .Include(p => p.Creator)
                .Include(p => p.OriginalPost)
                .ThenInclude(op => op.Creator)
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

        public async Task<PostResponseDto?> UpdatePost(int postId, PostCreationDto updatedPost, int currentUserId)
        {
            var Post = await _context.Posts.Include(p => p.Creator).FirstOrDefaultAsync(p => p.Id == postId);
            if (Post == null) return null;

            Post.Content = updatedPost.Content;
            Post.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            bool isLiked = _like.HasLikedPost(currentUserId, Post.Id);
            bool isFollowing = await _follow.IsUserFollowing(currentUserId, Post.UserId);
            return PostResponseDto.Create(Post, isLiked, isFollowing);
        }

        public async Task<PostResponseDto?> RetweetPost(int originalPostId, int userId)
        {
            var OriginalPost = await _context.Posts
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(p => p.Id == originalPostId);
            if (OriginalPost == null) return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

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
                OriginalPostId = originalPostId,
                MediaUploadPath = OriginalPost.MediaUploadPath,
                MediaUploadType = OriginalPost.MediaUploadType
            };
            
            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            var createdPost = await _context.Posts
                .Include(p => p.Creator)
                .FirstAsync(p => p.Id == newPost.Id);

            bool isLiked = _like.HasLikedPost(userId, createdPost.Id);
            bool isFollowing = await _follow.IsUserFollowing(userId, createdPost.UserId);
            return PostResponseDto.Create(createdPost, isLiked, isFollowing);
        }
    }
}