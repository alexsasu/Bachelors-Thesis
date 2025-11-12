using ApplicationCoreLibrary.Entities.Base;

namespace ApplicationCoreLibrary.Entities
{
    public class UrlReport : BaseEntity
    {
        public string? Url { get; set; }
        public string? Status { get; set; }
        public string? Domain { get; set; }
        public string? RegistrarName { get; set; }
        public string? RegistrarUrl { get; set; }
        public string? DomainCreationDate { get; set; }
        public string? DomainExpirationDate { get; set; }
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
