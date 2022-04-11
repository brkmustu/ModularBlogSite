using AutoMapper;
using UserPortalModule.Common;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.Extensions.Hashing;
using CoreModule.Domain.Users;
using System.ComponentModel.DataAnnotations;
using CoreModule.Application.Common.MessageContracts;
using MassTransit;
using System.Text.Json.Serialization;

namespace UserPortalModule.CommandHandlers;

public class UserRegistrationCommand : CommandRequest
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(70)]
    public string UserName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(20)]
    public string Password { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(70)]
    public string FirstName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(70)]
    public string LastName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(20)]
    public string MobileNumber { get; set; }

    [StringLength(70)]
    public string EmailAddress { get; set; }

    public long[] RoleIds { get; set; }

    [JsonIgnore]
    public override CrossCuttingConcerns[] ApplicableConcerns => new[]
    {
        CrossCuttingConcerns.Auditing,
        CrossCuttingConcerns.Validation
    };

    [JsonIgnore]
    public override string OperationName => "UserRegistrationCommand";

    public class Handler : UserPortalModuleApplicationService, ICommandHandler<UserRegistrationCommand>
    {
        public Handler(
                IUserPortalModuleDbContext dbContext, 
                IMapper mapper, 
                IPublishEndpoint publishEndpoint
            ) : base(dbContext, mapper, publishEndpoint)
        {
        }

        public async Task<Result> Handle(UserRegistrationCommand command)
        {
            var user = _mapper.Map<User>(command);

            var encryptedPassword = command.Password.CreatePasswordHash();

            user.SetPasswordHash(encryptedPassword.PasswordHash);
            user.SetPasswordSalt(encryptedPassword.PasswordSalt);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);

            await _publishEndpoint.Publish<UserRegisteredEvent>(new
            {
                User = _mapper.Map<UserDto>(user),
                Password = command.Password,
            });

            return Result.Success();
        }
    }
}

