using AutoMapper;
using ManagementModule.Common;

namespace ManagementModule
{
    public abstract class ManagementModuleApplicationService
    {
        protected readonly IManagementModuleDbContext _dbContext;
        protected readonly IMapper _mapper;

        protected ManagementModuleApplicationService(IManagementModuleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}

