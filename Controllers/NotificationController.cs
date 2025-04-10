using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBackEnd.Models;
using TwitterCloneBackEnd.Services;

namespace TwitterCloneBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notification ; 
        public NotificationController( INotificationRepository notification ) { _notification = notification ; }
        [Authorize]
        [HttpPost("{creatorUserId}/{receiverUserId}/{postId?}/{commentId?}/{followId?}")]
        public async Task<ActionResult<Notification?>> CreateNotification(int creatorUserId, int receiverUserId, [FromBody] NotificationType type, int? postId = null, int? commentId = null, int? followId = null)
        {
            var newNotification = await _notification.CreateNotification(creatorUserId,receiverUserId,type,postId,commentId,followId);
            if ( newNotification == null ) return BadRequest();
            return Ok(newNotification);
        }
        [Authorize]
        [HttpDelete("all/{userId}")]
        public async Task<IActionResult> DeleteAllNotificationsForUser(int userId)
        {
            var response = await _notification.DeleteAllNotificationsForUser(userId);
            if ( !response ) return BadRequest();
            return Ok("Notifications were successfully deleted");
        }
        [Authorize]
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            var response = await _notification.DeleteNotification(notificationId);
            if ( !response ) return BadRequest();
            return Ok("Notification was successfully deleted");
        }
        [Authorize]
        [HttpGet("{notificationId}")]
        public async Task<ActionResult<Notification>> GetNotificationById(int notificationId)
        {
            var notification = await _notification.GetNotificationById(notificationId);
            if ( notification == null ) return NotFound();
            return Ok(notification);
        }
        [Authorize]
        [HttpGet("all/{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsForUser(int userId)
        {
            var notifications = await _notification.GetNotificationsForUser(userId);
            if ( !notifications.Any() ) return Ok(new List<Notification>());
            return Ok(notifications);
        }
        [Authorize]
        [HttpGet("all/unread/{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetUnreadNotificationsForUser(int userId)
        {
            var unreadNotifications = await _notification.GetUnreadNotificationsForUser(userId);
            if ( !unreadNotifications.Any() ) return Ok(new List<Notification>()) ;
            return Ok(unreadNotifications);
        }
        [Authorize]
        [HttpPut("all/{userId}")]
        public async Task<IActionResult> MarkAllNotificationsAsRead(int userId)
        {
            var response = await _notification.MarkAllNotificationsAsRead(userId);
            if ( !response ) return BadRequest();
            return Ok("All notifications have been marked as read");
        }
        [Authorize]
        [HttpPut("{notificationId}")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            var response = await _notification.MarkNotificationAsRead(notificationId);
            if ( !response ) return BadRequest();
            return Ok("Notification has been marked as read");
        }
    }
}