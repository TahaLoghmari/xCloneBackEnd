using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TwitterCloneBackEnd.Hubs;
using TwitterCloneBackEnd.Models;
/*
This service has one job - sending messages:
- [SendNotificationToUser(userId, notification)]: Pushes notifications to specific users
- It finds the right user's group and sends data through their WebSocket connection
- This is what makes notifications appear instantly
*/
namespace TwitterCloneBackEnd.Services
{
    public class RealTimeNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public RealTimeNotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationToUser(int userId, Notification notification)
        {
            // Send to a specific user group
            await _hubContext.Clients.Group($"user_{userId}")
                .SendAsync("ReceiveNotification", notification);
        }
    }
}