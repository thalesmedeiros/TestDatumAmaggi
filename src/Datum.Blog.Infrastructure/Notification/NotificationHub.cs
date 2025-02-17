using Microsoft.AspNetCore.SignalR;

namespace Datum.Blog.Infrastructure.Notification
{
    public class NotificationHub : Hub
    {
        public async Task SendNewPostNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
