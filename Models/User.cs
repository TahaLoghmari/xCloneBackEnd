using Microsoft.AspNetCore.Identity;

namespace TwitterCloneBackEnd.Models
{
    public class User 
    {
        public int Id { get ; set ; } 
        public string UserName { get ; set ; } = string.Empty ; 
        public string Email { get ; set ; } = string.Empty ; 
        public string PasswordHash { get ; set ; } = string.Empty ; 
        public string DisplayName { get; set; } = string.Empty; 
        public string? ExternalProvider { get ; set ; }  
        public string? ExternalId { get ; set ; } 
        public string ImageUrl { get ; set ; } = string.Empty ; 
        public DateTime CreatedAt { get ; set ; } = DateTime.UtcNow ; 
        public int FollowerCount { get ; set ; } 
        public int FollowingCount { get ; set ; } 
        public DateTime BirthDate { get ; set ; }
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public ICollection<Follow> Following { get; set; } = new List<Follow>();
        public ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
        public ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
        public ICollection<Post> Posts { get ; set ; } = new List<Post>() ; 
        public ICollection<Comment> Comments { get ; set ; } = new List<Comment>();
        public ICollection<Like> Likes { get ; set ; } = new List<Like>();
    }
}