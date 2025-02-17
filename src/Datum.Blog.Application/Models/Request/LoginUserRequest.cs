using Datum.Blog.Application.Models.Response;
using MediatR;

namespace Datum.Blog.Application.Models.Request

{
    public class LoginUserRequest : IRequest<LoginUserResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginUserRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}

 