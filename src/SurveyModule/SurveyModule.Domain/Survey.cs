using CoreModule.Domain;

namespace SurveyModule
{
    public class Survey : AuditableEntity
    {
        public string SurveyName { get; set; }
        public string Notes { get; set; }
        public bool Kvkk { get; set; }
    }
}
