using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public interface IUserRepository 
    {
        Task<UserDto?> GetUserProfile(int userId , int currentUserId );
        Task<bool> DeleteUserProfile(int userId);
        Task<UserDto?> PutUserProfile(int userId, UserEditDto updatedUser , int currentUserId );
    }
}