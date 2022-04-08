using AutoMapper;
using ManagementModule.Common;
using MassTransit;

namespace ManagementModule
{
    public abstract class ManagementModuleApplicationService
    {
        protected readonly IManagementModuleDbContext _dbContext;
        protected readonly IMapper _mapper;
        protected readonly IPublishEndpoint _publishEndpoint;

        protected ManagementModuleApplicationService(IManagementModuleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        protected ManagementModuleApplicationService(IManagementModuleDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
    }
}

