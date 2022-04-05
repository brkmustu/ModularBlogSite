using AutoMapper;
using UserPortalModule.Common;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.Extensions.Hashing;
using CoreModule.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace UserPortalModule.CommandHandlers;

public class UserRegistrationAnonymousCommand : ICommand
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

    [Required(AllowEmptyStrings = false)]
    [StringLength(70)]
    public string EmailAddress { get; set; }

    public long[] RoleIds { get; set; }

    public class Handler : UserPortalModuleApplicationService, ICommandHandler<UserRegistrationAnonymousCommand>
    {
        public Handler(IUserPortalModuleDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<Result> Handle(UserRegistrationAnonymousCommand command)
        {
            var user = _mapper.Map<User>(command);

            var encryptedPassword = command.Password.CreatePasswordHash();

            user.SetPasswordHash(encryptedPassword.PasswordHash);
            user.SetPasswordSalt(encryptedPassword.PasswordSalt);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
            return Result.Success();
        }
    }
}

