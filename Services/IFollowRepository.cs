using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public interface IFollowRepository 
    {
        Task<FollowResponseDTO?> FollowUser( int followerId , int followingId );
        Task<bool> UnFollowUser( int followerId , int followingId );
        Task<bool> IsUserFollowing(int followerId, int followingId);
        Task<IEnumerable<UserDto>?> GetUserFollowers( int userId , int currentUserId);
        Task<IEnumerable<UserDto>?> GetUserFollowings( int userId , int currentUserId);
    }
}