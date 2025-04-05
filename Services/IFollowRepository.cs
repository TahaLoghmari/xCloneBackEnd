using TwitterCloneBackEnd.Models;

namespace TwitterCloneBackEnd.Services
{
    public interface IFollowRepository 
    {
        Task<Follow> FollowUser( int followerId , int followingId );
        Task<string> UnFollowUser( int followerId , int followingId );
        Task<IEnumerable<User>> GetUserFollowers( int userId );
        Task<IEnumerable<User>> GetUserFollowings( int userId );
    }
}