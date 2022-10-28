namespace CoreModule.Domain
{
    public class AuditableEntity : Entity
    {
        public int CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public int? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
