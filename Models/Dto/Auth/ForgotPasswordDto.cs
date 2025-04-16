namespace TwitterCloneBackEnd.Models.Dto;

public class ForgotPasswordDto
{
    public string Email { get ; set ; } = string.Empty; 
    public string Password { get ; set ; } = string.Empty ; 
    public string Token { get; set; } = string.Empty;
}