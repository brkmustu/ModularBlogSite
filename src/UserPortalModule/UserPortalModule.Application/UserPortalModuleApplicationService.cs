using AutoMapper;
using MassTransit;
using UserPortalModule.Common;

namespace UserPortalModule
{
    public abstract class UserPortalModuleApplicationService
    {
        protected readonly IUserPortalModuleDbContext _dbContext;
        protected readonly IMapper _mapper;
        protected readonly IPublishEndpoint _publishEndpoint;

        protected UserPortalModuleApplicationService(IUserPortalModuleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        protected UserPortalModuleApplicationService(IUserPortalModuleDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
    }
}

