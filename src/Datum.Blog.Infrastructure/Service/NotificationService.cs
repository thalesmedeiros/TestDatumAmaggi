using Datum.Blog.Domain.Interfaces;
using Datum.Blog.Infrastructure.Notification;
using Microsoft.AspNetCore.SignalR;

namespace Datum.Blog.Infrastructure.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
