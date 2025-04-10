namespace TwitterCloneBackEnd.Models
{
    public class Notification
    {
        public int Id { get ; set ; } // Notification Id 
        public int UserId { get ; set ; } // Id of who sent the notification 
        public int ReceiverUserId { get ; set ; } // the Id who received the notification 

        public DateTime CreatedAt { get ; set ; } = DateTime.UtcNow ; 

        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public int? FollowId { get; set; }

        public string Content { get ; set ; } = string.Empty ; 
        public bool IsRead { get ; set ; } 

        public User Creator { get ; set ; } = null! ;
        public User Receiver { get ; set ; } = null! ;

        public Post? RelatedPost { get; set; }
        public Comment? RelatedComment { get; set; }
        public Follow? RelatedFollow { get; set; }
        public NotificationType Type { get; set; }
    }
}