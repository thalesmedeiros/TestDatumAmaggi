using MediatR;
using Datum.Blog.Application.DTOs;

namespace Datum.Blog.Application.Queries.User;

public class GetAllUserQuery : IRequest<List<UserDto>>
{
}