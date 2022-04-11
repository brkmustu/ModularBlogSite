using AutoMapper;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.Extensions;
using CoreModule.Application.Extensions.Hashing;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UserPortalModule.Common;

namespace UserPortalModule.QueryHandlers;

public class UserLoginQuery : QueryRequest<AccessToken>
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }


    [JsonIgnore]
    public override CrossCuttingConcerns[] ApplicableConcerns => new[]
    {
        CrossCuttingConcerns.Auditing,
        CrossCuttingConcerns.Validation
    };

    [JsonIgnore]
    public override string OperationName => "UserLoginQuery";

    public class Handler : UserPortalModuleApplicationService, IQueryHandler<UserLoginQuery, AccessToken>
    {
        private readonly TokenOptions _tokenOptions;
        public Handler(IUserPortalModuleDbContext dbContext, IMapper mapper, IOptions<TokenOptions> tokenOptions) : base(dbContext, mapper)
        {
            _tokenOptions = tokenOptions.Value;
        }

        public Task<AccessToken> Handle(UserLoginQuery query)
        {
            var user = _dbContext.Users.Where(x => x.UserName.Equals(query.UserName)).FirstOrDefault();

            if (user == null) return Task.FromResult(default(AccessToken));

            if (query.Password.VerifyPasswordHash(user.PasswordHash, user.PasswordSalt))
            {
                var permissionIds = _dbContext.Roles.Where(x => user.RoleIds.Contains(x.Id)).SelectMany(x => x.PermissionIds);
                var permissions = _dbContext.Permissions.Where(x => permissionIds.Contains(x.Id)).ToList();
                var token = user.CreateToken(permissions.Select(x => x.Name), _tokenOptions);
                return Task.FromResult(token);
            }

            return Task.FromResult(default(AccessToken));
        }
    }
}
