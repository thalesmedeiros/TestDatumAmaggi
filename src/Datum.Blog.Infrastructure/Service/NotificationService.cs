using Datum.Blog.Domain.Interfaces;
using Datum.Blog.Infrastructure.Notification;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Datum.Blog.Infrastructure.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IHubContext<NotificationHub> hubContext, ILogger<NotificationService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyAsync(string message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
                _logger.LogInformation("Notificação enviada com sucesso: {Message}", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar a notificação: {Message}", message);

                throw new Exception("Erro ao enviar a notificação", ex);
            }
        }
    }
}
