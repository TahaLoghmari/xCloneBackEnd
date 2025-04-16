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
        public async Task<IEnumerable<PostResponseDto?>> GetUserPosts(int UserId)
        {
            return await _context.Posts
                .Include(p => p.Creator)
                .Include(p => p.OriginalPost)
                .ThenInclude(op => op.Creator)
                .Where(p => p.UserId == UserId)
                .Select(p => PostResponseDto.Create(p))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
        public async Task<PostResponseDto?> CreatePost(PostCreationDto newPostDto, int userId)
        {
            if ( newPostDto == null) return null ;

            var user = await _context.Users.FirstOrDefaultAsync( u => u.Id == userId );
            if ( user == null ) return null ;

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

            return PostResponseDto.Create(createdPost);
        }
        public async Task<bool> DeletePost(int postId)
        {
            var Post = await _context.Posts.FirstOrDefaultAsync( p => p.Id == postId );
            if ( Post == null ) return false ;

            _context.Posts.Remove(Post);
            await _context.SaveChangesAsync() ; 

            return true ;
        }
        public async Task<PostResponseDto?> GetPostById(int postId)
        {
            var Post = await _context.Posts
                .Include(p => p.Creator)
                .Include(p => p.OriginalPost)
                .ThenInclude(op => op.Creator)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (Post == null) return null; 

            return PostResponseDto.Create(Post);
        }
        public async Task<IEnumerable<PostResponseDto?>> GetAllPosts()
        {
            return await _context.Posts
                .Include(p => p.Creator)
                .Include(p => p.OriginalPost)
                .ThenInclude(op => op.Creator)
                .Select(p => PostResponseDto.Create(p))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
        public async Task<PostResponseDto?> UpdatePost(int postId, PostCreationDto updatedPost)
        {
            var Post = await _context.Posts.Include( p => p.Creator ).FirstOrDefaultAsync( p => p.Id == postId ) ;
            if ( Post == null ) return null ;

            Post.Content = updatedPost.Content ; 
            Post.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            return PostResponseDto.Create(Post);
        }
        public async Task<PostResponseDto?> RetweetPost(int originalPostId, int userId )
        {
            var OriginalPost = await _context.Posts
            .Include ( p => p.Creator )
            .FirstOrDefaultAsync( p => p.Id == originalPostId ) ;
            if (OriginalPost == null) return null ; 

            var user = await _context.Users.FirstOrDefaultAsync( u => u.Id == userId );
            if ( user == null ) return null ; 

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


           var createdPost = await _context.Posts
            .Include(p => p.Creator)
            .FirstAsync(p => p.Id == newPost.Id);

            return PostResponseDto.Create(createdPost);
        }
    }
}