namespace TwitterCloneBackEnd.Models.Dto ;

public class LikeResponseDTO
{
    public int Id { get ; set ; } 
    public int UserId { get ; set ; } 
    public int? PostId { get ; set ; } 
    public int? CommentId { get ; set ; }
    public UserDto Creator { get ; set ; } = null! ;
}