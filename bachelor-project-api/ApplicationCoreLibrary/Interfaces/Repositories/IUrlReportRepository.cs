using ApplicationCoreLibrary.Entities;

namespace ApplicationCoreLibrary.Interfaces.Repositories
{
    public interface IUrlReportRepository : IGenericRepository<UrlReport>
    {
        Task<UrlReport?> GetUrlReportOfUserAsync(string? url, int? userId);
        Task<List<UrlReport>> GetAllUrlReportsOfUserAsync(int id);
    }
}
