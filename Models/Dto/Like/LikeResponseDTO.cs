namespace TwitterCloneBackEnd.Models.Dto ;

public class LikeResponseDTO
{
    public int Id { get ; set ; } 
    public int UserId { get ; set ; } 
    public int? PostId { get ; set ; } 
    public int? CommentId { get ; set ; }
    public UserDto Creator { get ; set ; } = null! ;

    public static LikeResponseDTO? Create(Like like , bool followed )
    {
        if (like == null) return null;

        var userDto = like.Creator != null ? UserDto.Create(like.Creator,followed) : null;

        return new LikeResponseDTO
        {
            Id = like.Id,
            UserId = like.UserId,
            Creator = userDto! , 
            PostId = like.PostId,
            CommentId = like.CommentId
        };
    }
}