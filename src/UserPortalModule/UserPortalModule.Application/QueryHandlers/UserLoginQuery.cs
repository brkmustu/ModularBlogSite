using AutoMapper;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.CrossCuttingConcerns;
using CoreModule.Application.Extensions.Hashing;
using CoreModule.Domain.Permissions;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UserPortalModule.Common;
using UserPortalModule.Common.Contracts;

namespace UserPortalModule.QueryHandlers;

public class UserLoginQuery : QueryRequest<Result<AccessToken>>
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [JsonIgnore]
    public override string OperationName => "UserLoginQuery";

    [ValidationDecorator]
    [AuditingDecorator]
    public class Handler : UserPortalModuleApplicationService, IQueryHandler<UserLoginQuery, Result<AccessToken>>
    {
        private readonly TokenOptions _tokenOptions;
        public Handler(IUserPortalModuleDbContext dbContext, IMapper mapper, IOptions<TokenOptions> tokenOptions) : base(dbContext, mapper)
        {
            _tokenOptions = tokenOptions.Value;
        }

        public Task<Result<AccessToken>> Handle(UserLoginQuery query)
        {
            var user = _dbContext.Users.Where(x => x.UserName.Equals(query.UserName)).FirstOrDefault();

            if (user == null) 
                return Task.FromResult(Result.Failure<AccessToken>(new[] {"Kullanıcı bulunamadığından login işlemi gerçekleştirilemedi!"}));

            if (user.RoleIds == null || user.RoleIds.Length == 0) 
                return Task.FromResult(Result.Failure<AccessToken>(new[] { "Kullanıcı herhangi bir yetkiye sahip olmadığından login işlemi gerçekleştirilemedi!" }));

            if (query.Password.VerifyPasswordHash(user.PasswordSalt, user.PasswordHash))
            {
                List<long> permissionIds = new List<long>();
                List<Permission> permissions = new List<Permission>();

                foreach (var roleId in user.RoleIds)
                {
                    var rolePermissionIds = _dbContext.Roles.FirstOrDefault(x => x.Id == roleId).PermissionIds;
                    permissionIds.AddRange(rolePermissionIds);
                }
                foreach (var permissionId in permissionIds)
                {
                    var permission = _dbContext.Permissions.FirstOrDefault(x => x.Id == permissionId);
                    permissions.Add(permission);
                }



                var token = user.CreateToken(permissions.Select(x => x.Name), _tokenOptions);
                return Task.FromResult(Result.Success(token));
            }

            return Task.FromResult(Result.Failure<AccessToken>(new[] { "Şifre eşleştirilemediğinden login işlemi gerçekleştirilemedi!" }));
        }
    }
}
