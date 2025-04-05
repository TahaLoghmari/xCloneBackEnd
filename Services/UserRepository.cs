using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public class UserRepository : IUserRepository 
    {
        private readonly TwitterDbContext _context;
        public UserRepository(TwitterDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Notification>> GetUserNotifications(int UserId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == UserId)
                .ToListAsync();
        }
        public async Task<User> GetUserProfile(int UserId)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (User == null)
            {
                throw new KeyNotFoundException($"User with ID {UserId} was not found.");
            }
            return User;
        }
        public async Task<User> DeleteUserProfile(int UserId)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (User == null)
            {
                throw new KeyNotFoundException($"User with ID {UserId} was not found.");
            }
            _context.Users.Remove(User);
            await _context.SaveChangesAsync();
            return User;
        }

        public async Task<User> PutUserProfile(int UserId, UserEditDto updatedUser)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (User == null)
            {
                throw new KeyNotFoundException($"User with ID {UserId} was not found.");
            }
            User.Username = updatedUser.UserName;
            User.ImageUrl = updatedUser.ImageUrl;

            await _context.SaveChangesAsync();
            return User;
        }
    }
}