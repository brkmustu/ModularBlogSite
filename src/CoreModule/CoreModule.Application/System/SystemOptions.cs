namespace CoreModule.Application;

public class SystemOptions
{
    public readonly static Guid SystemGuid = new Guid("2fb12558-707d-4ccd-9bd0-f37de78359c0");
    public const string SectionName = "SystemOptions";
    public bool? SeedSampleData { get; set; }
}
