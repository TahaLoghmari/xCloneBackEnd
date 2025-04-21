using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public class FollowRepository : IFollowRepository
    {
        private readonly TwitterDbContext _context ; 
        private readonly INotificationRepository _notificationRepository;
        public FollowRepository( TwitterDbContext context , INotificationRepository notificationRepository) 
        {
            _context = context ; 
            _notificationRepository = notificationRepository;
        }
        public async Task<bool> IsUserFollowing(int followerId, int followingId)
        {
            return await _context.Follows.AnyAsync(f => 
                f.FollowerId == followerId && f.FollowingId == followingId);
        }
        public async Task<FollowResponseDTO?> FollowUser(int followerId, int followingId)
        {
            var follower = await _context.Users.FindAsync(followerId);
            var following = await _context.Users.FindAsync(followingId);
            if (follower == null || following == null)
                return null;
                
            var follow = await _context.Follows.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
            if(follow != null) return null;

            follow = new Follow
            {
                FollowerId = followerId,
                FollowingId = followingId
            };
            _context.Follows.Add(follow);

            follower.FollowingCount++;

            following.FollowerCount++;
            
            await _context.SaveChangesAsync();

            
            await _notificationRepository.CreateNotification(
                creatorUserId: followerId,
                receiverUserId: followingId,
                type: NotificationType.Follow,
                followId: follow.Id
            );

            follow = await _context.Follows
                .Include(f => f.Follower)
                .Include(f => f.Following)
                .FirstOrDefaultAsync(f => f.Id == follow.Id);

            return FollowResponseDTO.Create(follow,true);
        }

        public async Task<IEnumerable<UserDto>?> GetUserFollowers(int userId, int currentUserId)
        {
            var followers = await _context.Follows
                .Where(f => f.FollowingId == userId)
                .Include(f => f.Follower)
                .Select(f => f.Follower)
                .ToListAsync();

            var followedUserIds = await _context.Follows
                .Where(f => f.FollowerId == currentUserId)
                .Select(f => f.FollowingId)
                .ToListAsync();

            return followers.Select(follower => 
                UserDto.Create(follower, followedUserIds.Contains(follower.Id))
            ).ToList();
        
        }


        public async Task<IEnumerable<UserDto>?> GetUserFollowings(int userId, int currentUserId)
        {
            var followings = await _context.Follows
                .Where(f => f.FollowerId == userId)
                .Include(f => f.Following)
                .Select(f => f.Following)
                .ToListAsync();
            
            var followedUserIds = await _context.Follows
                .Where(f => f.FollowerId == currentUserId)
                .Select(f => f.FollowingId)
                .ToListAsync();
            
            return followings.Select(following => 
                UserDto.Create(following, followedUserIds.Contains(following.Id))
            ).ToList();
        }

        public async Task<bool> UnFollowUser(int followerId, int followingId)
        {
            var follower = await _context.Users.FindAsync(followerId);
            var following = await _context.Users.FindAsync(followingId);
    
            if (follower == null || following == null)
                return false;

            var follow = await _context.Follows.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
            if ( follow == null ) return false; 
            
            _context.Follows.Remove(follow);

            if (follower.FollowingCount > 0)
                follower.FollowingCount--;

            if (following.FollowerCount > 0)
                following.FollowerCount--;
                
            await _context.SaveChangesAsync();

            return true; 
        }
    }
}