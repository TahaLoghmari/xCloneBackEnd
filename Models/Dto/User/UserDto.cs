namespace TwitterCloneBackEnd.Models.Dto
{
    public class UserDto 
    {
        public int Id { get; set; } 
        public string DisplayName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; 
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get ; set ; } = DateTime.UtcNow ; 
        public int FollowerCount { get ; set ; } 
        public int FollowingCount { get ; set ; } 
        public DateTime BirthDate { get ; set ; }
        public static UserDto? Create(User user)
        {
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
                CreatedAt = user.CreatedAt,
                FollowerCount = user.FollowerCount,
                FollowingCount = user.FollowingCount,
                BirthDate = user.BirthDate
            };
        }

    }
}