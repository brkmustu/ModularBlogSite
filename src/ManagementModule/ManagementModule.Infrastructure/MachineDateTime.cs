using CoreModule.Common;

namespace ManagementModule.Infrastructure;

public class MachineDateTime : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}

