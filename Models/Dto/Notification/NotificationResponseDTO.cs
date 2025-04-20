namespace TwitterCloneBackEnd.Models.Dto ;

public class NotificationResponseDTO 
{
    public int Id { get ; set ; } 
    public int UserId { get ; set ; } 
    public int ReceiverUserId { get ; set ; } 

    public DateTime CreatedAt { get ; set ; } = DateTime.UtcNow ; 

    public int? PostId { get; set; }
    public int? CommentId { get; set; }
    public int? FollowId { get; set; }

    public string Content { get ; set ; } = string.Empty ; 
    public bool IsRead { get ; set ; } 

    public UserDto Creator { get ; set ; } = null! ;
    public UserDto Receiver { get ; set ; } = null! ;

    public PostResponseDto? RelatedPost { get; set; }
    public CommentResponseDto? RelatedComment { get; set; }
    public FollowResponseDTO? RelatedFollow { get; set; }
    public NotificationType Type { get; set; }
    public static NotificationResponseDTO? Create(Notification notification)
    {
        if (notification == null) return null;
        
        bool creatorFollowed = false;
        bool receiverFollowed = false;
        bool postLiked = false;
        
        return new NotificationResponseDTO
        {
            Id = notification.Id,
            UserId = notification.UserId,
            ReceiverUserId = notification.ReceiverUserId,
            CreatedAt = notification.CreatedAt,
            PostId = notification.PostId,
            CommentId = notification.CommentId,
            FollowId = notification.FollowId,
            Content = notification.Content,
            IsRead = notification.IsRead,
            Type = notification.Type,
            Creator = notification.Creator != null ? UserDto.Create(notification.Creator, creatorFollowed)! : null!,
            Receiver = notification.Receiver != null ? UserDto.Create(notification.Receiver, receiverFollowed)! : null!,
            RelatedPost = notification.RelatedPost != null ? PostResponseDto.Create(notification.RelatedPost, postLiked, false) : null,
            RelatedComment = notification.RelatedComment != null ? CommentResponseDto.Create(notification.RelatedComment, false, false) : null,
            RelatedFollow = notification.RelatedFollow != null ? FollowResponseDTO.Create(notification.RelatedFollow, false) : null
        };
    }
}