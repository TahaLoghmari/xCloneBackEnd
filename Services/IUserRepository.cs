using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public interface IUserRepository 
    {
        Task<IEnumerable<Notification>> GetUserNotifications(int userId);
        Task<User> GetUserProfile(int userId);
        Task<User> DeleteUserProfile(int userId);
        Task<User> PutUserProfile(int userId, UserEditDto updatedUser);
    }
}