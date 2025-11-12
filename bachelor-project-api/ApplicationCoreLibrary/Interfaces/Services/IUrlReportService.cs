using ApplicationCoreLibrary.DTOs;

namespace ApplicationCoreLibrary.Interfaces.Services
{
    public interface IUrlReportService
    {
        Task<GetUrlReportDto> GetUrlReportById(int id);
        Task<GetUrlReportDto> GetUrlReportOfUser(string? url, int? userId);
        Task<List<GetUrlReportDto>> GetAllUrlReportsOfUser(int id);
        Task<GetUrlReportDto> DeleteUrlReportById(int id);
    }
}
