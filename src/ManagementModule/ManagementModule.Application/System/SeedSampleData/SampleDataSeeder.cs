using AutoMapper;
using Microsoft.Extensions.Options;
using ManagementModule.Common;
using CoreModule.Domain.Users;

namespace ManagementModule.System.SeedSampleData
{
    public class SampleDataSeeder
    {
        private readonly IManagementModuleDbContext _context;
        private readonly IMapper _mapper;
        private readonly SystemOptions _options;

        public SampleDataSeeder(IManagementModuleDbContext context, IMapper mapper, IOptions<SystemOptions> options)
        {
            _context = context;
            _mapper = mapper;
            _options = options.Value;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            if (_options != null && _options.SeedSampleData.HasValue && _options.SeedSampleData.Value)
            {
                /// seed sample datas
                /// 

                var adminUser = _context.Users.Where(x => x.UserName == "admin").FirstOrDefault();

                if (adminUser != null) return;

                User admin = new User("admin", "admin", "admin", new long[] { 1, 2, 3, 4, 5 });

                admin.Id = Guid.NewGuid();

                _context.Users.Add(admin);

                await _context.SaveChangesAsync(cancellationToken: CancellationToken.None);
            }
        }
    }
}

