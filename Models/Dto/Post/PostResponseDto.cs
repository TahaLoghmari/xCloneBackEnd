using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Models ;

public class PostResponseDto
{
    public int Id { get ; set ; } 
    public int UserId { get ; set ; }

    public int CommentsCount { get ; set ; } 
    public int SharesCount { get ; set ; } 
    public int LikesCount { get ; set ; }
    public int ViewsCount { get ; set ; }

    public string? MediaUploadPath { get ; set ; }
    public string? MediaUploadType { get ; set ; }

    public string Content { get ; set ; } = string.Empty ; 
    public DateTime CreatedAt { get ; set ; } = DateTime.UtcNow ;
    public DateTime? UpdatedAt { get; set; }

    public int? OriginalPostId { get; set; }
    public bool IsRetweet { get; set; }
    public bool HasLiked { get ; set ; } 
    public PostResponseDto? OriginalPost { get; set; }
    public UserDto Creator { get ; set ; } = null! ;
    public static PostResponseDto? Create(Post post , bool Liked , bool followed )
    {
        if (post == null) return null;

        var userDto = post.Creator != null ? UserDto.Create(post.Creator,followed) : null;

        return new PostResponseDto
        {
            Id = post.Id,
            UserId = post.UserId,
            Content = post.Content,
            MediaUploadPath = post.MediaUploadPath ?? string.Empty,
            MediaUploadType = post.MediaUploadType ?? string.Empty,
            CommentsCount = post.CommentsCount,
            SharesCount = post.SharesCount,
            LikesCount = post.LikesCount,
            ViewsCount = post.ViewsCount,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            IsRetweet = post.IsRetweet,
            OriginalPostId = post.OriginalPostId,
            Creator = userDto! , 
            OriginalPost = post.OriginalPost != null ? Create(post.OriginalPost,false,false) : null,
            HasLiked = Liked
        };
    }

}