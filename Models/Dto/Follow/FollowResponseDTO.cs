namespace TwitterCloneBackEnd.Models.Dto;

public class FollowResponseDTO
{
    public int Id { get ; set ; } 
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
    public DateTime CreatedAt { get ; set ; } = DateTime.UtcNow ; 
    public UserDto Follower { get ; set ; } = null! ; 
    public UserDto Following { get ; set ; } = null! ;

    public static FollowResponseDTO? Create(Follow follow , bool followed )
    {
        if ( follow == null) return null;

        var followerDto = follow.Follower != null ? UserDto.Create(follow.Follower,followed) : null;
        var followingDto = follow.Following != null ? UserDto.Create(follow.Following,followed) : null;

        return new FollowResponseDTO
        {
            Id = follow.Id,
            FollowerId = follow.FollowerId , 
            FollowingId = follow.FollowingId,
            CreatedAt = follow.CreatedAt,
            Follower = followerDto !, 
            Following = followingDto ! ,
        };
    } 
}