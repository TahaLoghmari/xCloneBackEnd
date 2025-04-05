using TwitterCloneBackEnd.Models;

namespace TwitterCloneBackEnd.Services
{
    public interface INotificationRepository
    {
        Task<Notification?> CreateNotification(int creatorUserId, int receiverUserId, NotificationType type, 
            int? postId = null, int? commentId = null, int? followId = null);
        Task<IEnumerable<Notification>> GetNotificationsForUser(int userId);
        Task<Notification?> GetNotificationById(int notificationId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsForUser(int userId);
        Task<bool> MarkNotificationAsRead(int notificationId);
        Task<bool> MarkAllNotificationsAsRead(int userId);
        Task<bool> DeleteNotification(int notificationId);
        Task<bool> DeleteAllNotificationsForUser(int userId);
    }
}