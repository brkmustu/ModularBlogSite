using AutoMapper;
using ManagementModule.Common;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.Extensions;
using System.ComponentModel.DataAnnotations;

namespace ManagementModule.QueryHandlers;

public class GetUserListQuery : IQuery<Paged<UserDto>>
{
    public PageInfo Paging { get; set; } = new PageInfo { PageSize = 10, PageIndex = 0 };

    public bool? IsActive { get; set; }

    [Range(1, 5)]
    public int? UserStatusId { get; set; }

    public class Handler : ManagementModuleApplicationService, IQueryHandler<GetUserListQuery, Paged<UserDto>>
    {
        public Handler(IManagementModuleDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<Paged<UserDto>> Handle(GetUserListQuery query)
        {
            var users = _dbContext.Users
                        .Where(user => query.IsActive.HasValue ? query.IsActive.Value == user.IsActive : true)
                        .ToArray();

            if (query.UserStatusId.HasValue)
            {
                return users
                    .Where(x => x.UserStatusId == query.UserStatusId.Value)
                    .Select(user => _mapper.Map<UserDto>(user))
                    .Page(query.Paging);
            }

            return await Task.FromResult(users.Select(user => _mapper.Map<UserDto>(user)).Page(query.Paging));
        }
    }
}

