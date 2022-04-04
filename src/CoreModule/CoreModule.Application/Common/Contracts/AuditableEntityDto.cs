namespace CoreModule.Application.Common.Contracts;

public class AuditableEntityDto : EntityDto
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
public class FullAuditableEntityDto : AuditableEntityDto
{
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}
