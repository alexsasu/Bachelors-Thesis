using ApplicationCoreLibrary.Entities;

namespace ApplicationCoreLibrary.DTOs
{
    public class GetUrlReportDto
    {
        public string? Url { get; set; }
        public string? Status { get; set; }
        public string? Domain { get; set; }
        public string? RegistrarName { get; set; }
        public string? RegistrarUrl { get; set; }
        public string? DomainCreationDate { get; set; }
        public string? DomainExpirationDate { get; set; }

        public GetUrlReportDto() { }

        public GetUrlReportDto(UrlReport report)
        {
            Url = report.Url;
            Status = report.Status;
            Domain = report.Domain;
            RegistrarName = report.RegistrarName;
            RegistrarUrl = report.RegistrarUrl;
            DomainCreationDate = report.DomainCreationDate;
            DomainExpirationDate = report.DomainExpirationDate;
        }
    }
}
