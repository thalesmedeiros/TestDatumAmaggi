namespace Datum.Blog.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAsync(string email, string password);
    }
}