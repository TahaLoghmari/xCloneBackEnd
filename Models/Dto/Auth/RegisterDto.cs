namespace TwitterCloneBackEnd.Models.Dto
{
    public class RegisterDto 
    {
        public string DisplayName { get ; set ; } = string.Empty ; 
        public string UserName { get ; set ; } = string.Empty;
        public string Email { get ; set ; } = string.Empty;
        public string Password { get ; set ; } = string.Empty ;
        public string ImageUrl { get ; set ; } = string.Empty ; 
        public DateTime BirthDate { get ; set ; } 
    }
}