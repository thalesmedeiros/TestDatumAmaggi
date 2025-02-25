﻿using AutoMapper;
using Datum.Blog.Application.Commands.Post;
using Datum.Blog.Application.Commands.User;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Domain.Entities;

namespace Datum.Blog.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Post, PostDto>().ReverseMap();
        CreateMap<CreatePostCommand, PostDto>();
        CreateMap<UpdatePostCommand, PostDto>();

        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<CreateUserCommand, UserDto>();
        CreateMap<UpdateUserCommand, UserDto>();
    }
}