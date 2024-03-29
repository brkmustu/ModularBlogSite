﻿using AutoMapper;
using UserPortalModule.CommandHandlers;
using CoreModule.Domain.Users;
using CoreModule.Application.Common.Contracts;

namespace UserPortalModule
{
    public class UserPortalModuleAutoMapperProfile : Profile
    {
        public UserPortalModuleAutoMapperProfile()
        {
            CreateMap<UserRegistrationCommand, User>();
            CreateMap<User, UserRegistrationCommand>();

            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}

