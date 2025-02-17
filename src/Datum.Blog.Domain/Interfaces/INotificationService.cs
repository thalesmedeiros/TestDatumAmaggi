namespace Datum.Blog.Domain.Interfaces
{
    public interface INotificationService
    {
        Task NotifyAsync(string message);
    }
}
