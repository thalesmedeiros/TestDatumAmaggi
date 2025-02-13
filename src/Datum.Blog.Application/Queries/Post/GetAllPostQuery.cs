using MediatR;
using Datum.Blog.Application.DTOs;

namespace Datum.Blog.Application.Queries.Post;

public class GetAllPostQuery : IRequest<List<PostDto>>
{
}