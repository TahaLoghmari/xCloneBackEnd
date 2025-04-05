namespace TwitterCloneBackEnd.Models
{
    public class Comment 
    {
        public int Id { get ; set ; }
        public int UserId { get ; set ; } 
        public int PostId { get ; set ; }
        public int? ParentCommentId { get ; set ; } 

        public DateTime CreatedAt { get ; set ; } = DateTime.UtcNow ; 
        public DateTime? UpdatedAt { get ; set ; } 

        public string Content { get ; set ; } = string.Empty ;

        public int LikesCount { get ; set ; } 
        public int RepliesCount { get ; set; }

        public ICollection<Comment> Replies { get ; set ; } = new List<Comment>();
        public ICollection<Like> Likes { get ; set ; } = new List<Like>();
        public ICollection<Notification> Notifications { get ; set ; } = new List<Notification>();
        public User Creator { get ; set ; } = null! ; 
        public Post RelatedPost { get ; set ; } = null! ; 
        public Comment? ParentComment { get ; set ; } 

    }
}