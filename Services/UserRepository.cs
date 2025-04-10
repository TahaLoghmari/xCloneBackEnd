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
        public async Task<UserDto?> GetUserProfile(int UserId)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (User == null) return null ;
            
            var UserDto = new UserDto {
                Id = User.Id ,
                DisplayName = User.DisplayName,
                Username = User.UserName ?? string.Empty,
                Email = User.Email!,
                ImageUrl = User.ImageUrl
            };
            return UserDto;
        }
        public async Task<bool> DeleteUserProfile(int UserId)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (User == null)
            {
                return false ; 
            }
            _context.Users.Remove(User);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto?> PutUserProfile(int UserId, UserEditDto updatedUser)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            
            if (User == null) return null ; 

            User.UserName = updatedUser.UserName;
            User.ImageUrl = updatedUser.ImageUrl;
            var UserDto = new UserDto {
                Id = User.Id ,
                DisplayName = User.DisplayName,
                Username = User.UserName,
                Email = User.Email!,
                ImageUrl = User.ImageUrl
            };
            await _context.SaveChangesAsync();
            return UserDto;
        }
    }
}