using ApplicationCoreLibrary.DTOs;
using ApplicationCoreLibrary.Helpers;

namespace ApplicationCoreLibrary.Interfaces.Services
{
    public interface IHomeService
    {
        UrlAnalysis GetUrlAnalysis(string? url);
        Task<UrlAnalysis> AddOrUpdateUrlReport(string? url, int? userId);
        Task<UrlAnalysis> AddUrlReport(string? url, int? userId);
        Task<UrlAnalysis> UpdateUrlReport(string? url, int? userId);
        Task<GetUrlReportDto> GetUrlReportOfUser(string? url, int? userId);
    }
}
