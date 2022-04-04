using CoreModule.Common;

namespace UserPortalModule.Infrastructure;

public class MachineDateTime : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}

