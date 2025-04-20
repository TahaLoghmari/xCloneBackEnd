using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public interface INotificationRepository
    {
        Task<NotificationResponseDTO?> CreateNotification(int creatorUserId, int receiverUserId, NotificationType type, 
            int? postId = null, int? commentId = null, int? followId = null);
        Task<IEnumerable<NotificationResponseDTO>> GetNotificationsForUser(int userId);
        Task<NotificationResponseDTO?> GetNotificationById(int notificationId);
        Task<IEnumerable<NotificationResponseDTO>> GetUnreadNotificationsForUser(int userId);
        Task<bool> MarkNotificationAsRead(int notificationId);
        Task<bool> MarkAllNotificationsAsRead(int userId);
        Task<bool> DeleteNotification(int notificationId);
        Task<bool> DeleteAllNotificationsForUser(int userId);
    }
}