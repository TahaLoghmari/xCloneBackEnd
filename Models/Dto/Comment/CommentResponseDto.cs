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
    public bool HasLiked { get ; set ; } 
    public UserDto Creator { get ; set ; } 
    public static CommentResponseDto? Create(Comment comment , bool Liked , bool followed )
    {
        if ( comment == null) return null;

        var userDto = comment.Creator != null ? UserDto.Create(comment.Creator,followed) : null;

        return new CommentResponseDto
        {
            Id = comment.Id,
            UserId = comment.UserId,
            Content = comment.Content,
            PostId = comment.PostId,
            ParentCommentId = comment.ParentCommentId ,
            RepliesCount = comment.RepliesCount,
            LikesCount = comment.LikesCount,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            Creator = userDto! , 
            HasLiked = Liked
        };
    }
}