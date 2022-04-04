﻿using AutoMapper;
using Microsoft.Extensions.Options;
using UserPortalModule.Common;

namespace UserPortalModule.System.SeedSampleData
{
    public class SampleDataSeeder
    {
        private const string UserId = "84054f6f-384c-4f8e-899a-f8872bd3d207";
        private readonly IUserPortalModuleDbContext _context;
        private readonly IMapper _mapper;
        private readonly SystemOptions _options;

        public SampleDataSeeder(IUserPortalModuleDbContext context, IMapper mapper, IOptions<SystemOptions> options)
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

            }
        }
    }
}

