namespace TwitterCloneBackEnd.Models
{
    public class Post 
    {
        public int Id { get ; set ; } 
        public int UserId { get ; set ; }

        public int CommentsCount { get ; set ; } 
        public int SharesCount { get ; set ; } 
        public int LikesCount { get ; set ; }
        public int ViewsCount { get ; set ; }

        public string? MediaUploadPath { get ; set ; }
        public string? MediaUploadType { get ; set ; }

        public string Content { get ; set ; } = string.Empty ; 
        public DateTime CreatedAt { get ; set ; } = DateTime.UtcNow ;
        public DateTime? UpdatedAt { get; set; }

        public int? OriginalPostId { get; set; }
        public Post? OriginalPost { get; set; }
        public bool IsRetweet { get; set; }

        public ICollection<Comment> Comments { get ; set ; } = new List<Comment>() ;
        public ICollection<Notification> Notifications { get ; set ; } = new List<Notification>();
        public ICollection<Like> Likes { get ; set ; } = new List<Like>();
        public ICollection<Post> Retweets { get; set; } = new List<Post>();
        public User Creator { get ; set ; } = null! ;
    }
}