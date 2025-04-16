namespace TwitterCloneBackEnd.Models.Dto;

public class CommentResponseDto 
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public int? ParentCommentId { get ; set ; } 
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get ; set ; } = DateTime.UtcNow ; 
    public DateTime? UpdatedAt { get ; set ; }
    public int LikesCount { get ; set ; } 
    public int RepliesCount { get ; set; }
    public UserDto Creator { get ; set ; } 
}