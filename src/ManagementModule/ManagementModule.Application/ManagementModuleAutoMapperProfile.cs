using AutoMapper;
using CoreModule.Domain.Users;
using CoreModule.Application.Common.Contracts;

namespace ManagementModule
{
    public class ManagementModuleAutoMapperProfile : Profile
    {
        public ManagementModuleAutoMapperProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}

