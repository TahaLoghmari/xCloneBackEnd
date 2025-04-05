using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;

namespace TwitterCloneBackEnd.Services
{
    public class FollowRepository : IFollowRepository
    {
        private readonly TwitterDbContext _context ; 
        public FollowRepository( TwitterDbContext context ) 
        {
            _context = context ; 
        }

        public async Task<Follow> FollowUser(int followerId, int followingId)
        {
            var follow = await _context.Follows.FirstOrDefaultAsync( f => f.FollowerId == followerId && f.FollowingId == followingId);
            if(follow == null)
            {
                throw new KeyNotFoundException($"No follow record found for followerId {followerId} and followingId {followingId}");
            }
            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();
            return follow;
        }

        public async Task<IEnumerable<User>> GetUserFollowers(int userId)
        {
            return await _context.Follows
                .Where(f => f.FollowingId == userId)
                .Select(f => f.Follower) 
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUserFollowings(int userId)
        {
            return await _context.Follows
                .Where(f => f.FollowerId == userId)
                .Select(f => f.Following) 
                .ToListAsync();
        }

        public async Task<string> UnFollowUser(int followerId, int followingId)
        {
            var follow = await _context.Follows.FirstOrDefaultAsync( f => f.FollowerId == followerId && f.FollowingId == followingId);
            if(follow == null)
            {
                throw new KeyNotFoundException($"No follow record found for followerId {followerId} and followingId {followingId}");
            }
            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
            return $"{await _context.Users.FirstOrDefaultAsync( u => u.Id == followerId)} Unfollowed {await _context.Users.FirstOrDefaultAsync( u => u.Id == followingId)}";
        }
    }
}