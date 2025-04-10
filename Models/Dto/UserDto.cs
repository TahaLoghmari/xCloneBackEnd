namespace TwitterCloneBackEnd.Models.Dto
{
    public class UserDto 
    {
        public int Id { get; set; } 
        public string DisplayName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; 
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get ; set ; } = DateTime.UtcNow ; 
        public int FollowerCount { get ; set ; } 
        public int FollowingCount { get ; set ; } 
        public DateTime BirthDate { get ; set ; }
    }
}