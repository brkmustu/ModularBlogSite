using AutoMapper;
using ManagementModule.Common;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ManagementModule.QueryHandlers;

public class GetUserListQuery : QueryRequest<GetUserListQueryResult>
{
    public PageInfo Paging { get; set; } = new PageInfo { PageSize = 10, PageIndex = 0 };

    public bool? IncludePermissions { get; set; } = false;

    public bool? IsActive { get; set; }

    [Range(1, 5)]
    public int? UserStatusId { get; set; }

    [JsonIgnore]
    public override CrossCuttingConcerns[] ApplicableConcerns => new[]
    {
        CrossCuttingConcerns.Auditing,
        CrossCuttingConcerns.Authorization,
        CrossCuttingConcerns.Validation
    };

    [JsonIgnore]
    public override string OperationName => "GetUserListQuery";

    public class Handler : ManagementModuleApplicationService, IQueryHandler<GetUserListQuery, GetUserListQueryResult>
    {
        public Handler(IManagementModuleDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<GetUserListQueryResult> Handle(GetUserListQuery query)
        {
            var users = _dbContext.Users
                        .Where(user => query.IsActive.HasValue ? query.IsActive.Value == user.IsActive : true)
                        .ToArray();

            if (query.UserStatusId.HasValue)
            {
                users = users.Where(x => x.UserStatusId == query.UserStatusId.Value)
                    .ToArray();
            }

            Dictionary<Guid, long[]> permissions = new Dictionary<Guid, long[]>();

            //if (query.IncludePermissions.HasValue && query.IncludePermissions.Value)
            //{
            //    foreach (var user in users)
            //    {
            //        if (user.RoleIds is not null and user.RoleIds.Any())
            //        {

            //        }
            //        //_dbContext.Roles.Where(x => user.RoleIds)
            //    }
            //    //var _permissions = _dbContext.Roles.Where(x => users.sele)
            //}

            return await Task.FromResult(new GetUserListQueryResult
            {
                Users = users.Select(user => _mapper.Map<UserDto>(user)).Page(query.Paging),
                Permissions = permissions
            });
        }
    }
}

public class GetUserListQueryResult
{
    public Paged<UserDto> Users { get; set; }
    public Dictionary<Guid, long[]> Permissions { get; set; }
}
