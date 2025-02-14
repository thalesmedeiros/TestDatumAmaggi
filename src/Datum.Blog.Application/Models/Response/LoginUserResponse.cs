 namespace Datum.Blog.Application.Models.Response
{
    public class LoginUserResponse
    {
        public string Token { get; set; }

        public LoginUserResponse(string token)
        {
            Token = token;
        }
    }
}

 