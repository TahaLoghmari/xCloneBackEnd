using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
/*
This is the central connection point for WebSockets:
- When a user logs in, their browser connects to this hub
- [JoinUserGroup(userId)]: Groups users by ID so notifications go only to the right person
- Each user is placed in their own channel: `user_{userId}`
*/
namespace TwitterCloneBackEnd.Hubs
{
    public class NotificationHub : Hub
    {
        // This method allows clients to join a user-specific notification group
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        // Optional: Method for leaving the group
        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
    }
}