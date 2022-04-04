using AutoMapper;
using UserPortalModule.Common;

namespace UserPortalModule
{
    public abstract class UserPortalModuleApplicationService
    {
        protected readonly IUserPortalModuleDbContext _dbContext;
        protected readonly IMapper _mapper;

        protected UserPortalModuleApplicationService(IUserPortalModuleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}

