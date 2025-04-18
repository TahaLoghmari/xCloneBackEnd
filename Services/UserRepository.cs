using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using CloudinaryDotNet;
using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public class UserRepository : IUserRepository 
    {
        private readonly TwitterDbContext _context;
        private readonly IFollowRepository _follow ; 
        
        public UserRepository(TwitterDbContext context  , IFollowRepository follow )
        {
            _context = context;
            _follow = follow;
        }
        public async Task<UserDto?> GetUserProfile(int UserId , int currentUserId )
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (User == null) return null ;
        
            var followed = await _follow.IsUserFollowing(currentUserId,UserId);

            return UserDto.Create(User,followed);
        }
        public async Task<bool> DeleteUserProfile(int UserId)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (User == null) return false ; 
            _context.Users.Remove(User);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto?> PutUserProfile(int UserId, UserEditDto updatedUser , int currentUserId )
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            
            if (User == null) return null ; 

            User.UserName = updatedUser.UserName;
            User.ImageUrl = updatedUser.ImageUrl;

            var followed = await _follow.IsUserFollowing(currentUserId,UserId);

            await _context.SaveChangesAsync();

            return UserDto.Create(User,followed);
        }
    }
}