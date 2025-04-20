using Microsoft.EntityFrameworkCore;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Models.Data;
using TwitterCloneBackEnd.Models.Dto;

namespace TwitterCloneBackEnd.Services
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly TwitterDbContext _context ;
        public NotificationRepository( TwitterDbContext context  )
        {
            _context = context ; 
        }

        public async Task<NotificationResponseDTO?> CreateNotification(int creatorUserId, int receiverUserId, NotificationType type, int? postId = null, int? commentId = null, int? followId = null)
        {
            User? receiver = await _context.Users.FirstOrDefaultAsync( u => u.Id == receiverUserId );
            User? creator = await _context.Users.FirstOrDefaultAsync( u => u.Id == creatorUserId ); 
            if ( receiver == null || creator == null ) return null ; 
            Post? post = await _context.Posts.FirstOrDefaultAsync( p => p.Id == postId );
            Comment? comment = await _context.Comments.FirstOrDefaultAsync( c => c.Id == commentId );
            Follow? follow = await _context.Follows.FirstOrDefaultAsync( f => f.Id == followId );
            string message = "";
            if ( type == NotificationType.Like )
            {
                if ( comment != null ) message = $"@{creator.UserName} liked your Reply : '{comment.Content}'."; 
                else if ( post != null ) message = $"@{creator.UserName} liked your Tweet : '{post.Content}'.";
                else return null ;
            }
            else if ( type == NotificationType.Retweet )
            {
                if ( post != null ) message = $"@{creator.UserName} retweeted your Tweet: '{post.Content}'.";
                else return null ;
            }
            else if ( type == NotificationType.Reply )
            {
                if ( comment != null ) message = $"@{creator.UserName} replied to your Reply : '{comment.Content}'.";
                else if ( post != null ) message = $"@{creator.UserName} replied to your Tweet : '{post.Content}'.";
                else return null ;
            }
            else if ( type == NotificationType.Mention )
            {
                if ( post != null ) message = $"@{creator.UserName} mentioned you in a Tweet: '{post.Content}'.";
                else if ( comment != null ) message = $"@{creator.UserName} mentioned you in a Reply: '{comment.Content}'.";
                else return null ;
            }
            else message = $"@{creator.UserName} followed you.";
            
            var newNotification = new Notification {
                UserId = creatorUserId,
                ReceiverUserId = receiverUserId,
                PostId = postId,
                CommentId = commentId,
                FollowId = followId,
                Content = message,
                IsRead = false ,
                Type = type
            };
            
            _context.Notifications.Add(newNotification);
            await _context.SaveChangesAsync();
            
            await _context.Entry(newNotification).Reference(n => n.Creator).LoadAsync();
            await _context.Entry(newNotification).Reference(n => n.Receiver).LoadAsync();
            
            if (newNotification.PostId.HasValue)
                await _context.Entry(newNotification).Reference(n => n.RelatedPost).LoadAsync();
            
            if (newNotification.CommentId.HasValue)
                await _context.Entry(newNotification).Reference(n => n.RelatedComment).LoadAsync();
            
            if (newNotification.FollowId.HasValue)
                await _context.Entry(newNotification).Reference(n => n.RelatedFollow).LoadAsync();
                
            return NotificationResponseDTO.Create(newNotification);
        }
        public async Task<bool> DeleteAllNotificationsForUser(int userId)
        {
            var notifications = await _context.Notifications.Where( n => n.ReceiverUserId == userId ).ToListAsync();
            if ( !notifications.Any() ) return false ; 
            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
            return true ; 
        }
        public async Task<bool> DeleteNotification(int notificationId)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync( n => n.Id == notificationId );
            if ( notification == null ) return false ; 
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true ; 
        }
        public async Task<NotificationResponseDTO?> GetNotificationById(int notificationId)
        {
            var notification = await _context.Notifications
            .Include(n => n.Creator)
            .Include(n => n.Receiver)
            .Include(n => n.RelatedPost)
            .Include(n => n.RelatedComment)
            .Include(n => n.RelatedFollow)
            .FirstOrDefaultAsync( n => n.Id == notificationId );
            return NotificationResponseDTO.Create(notification);
        }
        public async Task<IEnumerable<NotificationResponseDTO>> GetNotificationsForUser(int userId)
        {
            var notifications = await _context.Notifications.Where( n => n.ReceiverUserId == userId )
            .Include(n => n.Creator)
            .Include(n => n.Receiver)
            .Include(n => n.RelatedPost)
            .Include(n => n.RelatedComment)
            .Include(n => n.RelatedFollow)
            .ToListAsync();
            return notifications.Select(n => NotificationResponseDTO.Create(n)).Where(n => n != null);
        }
        public async Task<IEnumerable<NotificationResponseDTO>> GetUnreadNotificationsForUser(int userId)
        {
            var notifications = await _context.Notifications.Where( n => n.ReceiverUserId == userId && n.IsRead == false)
            .Include(n => n.Creator)
            .Include(n => n.Receiver)
            .Include(n => n.RelatedPost)
            .Include(n => n.RelatedComment)
            .Include(n => n.RelatedFollow)
            .ToListAsync();
            return notifications.Select(n => NotificationResponseDTO.Create(n)).Where(n => n != null);
        }
        public async Task<bool> MarkAllNotificationsAsRead(int userId)
        {
            var notifications = await _context.Notifications.Where( n => n.ReceiverUserId == userId ).ToListAsync();
            
            if ( !notifications.Any() ) return false ;

            foreach (var notification in notifications) notification.IsRead = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkNotificationAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync( n => n.Id == notificationId );
            if ( notification == null ) return false ;
            notification.IsRead = true ; 
            await _context.SaveChangesAsync();
            return true ; 
        }
    }
}