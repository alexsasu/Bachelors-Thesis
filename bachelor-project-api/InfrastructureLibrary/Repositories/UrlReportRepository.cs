using ApplicationCoreLibrary.Entities;
using ApplicationCoreLibrary.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLibrary.Repositories
{
    public class UrlReportRepository : GenericRepository<UrlReport>, IUrlReportRepository
    {
        public UrlReportRepository(ProjDbContext context) : base(context) { }

        public async Task<UrlReport?> GetUrlReportOfUserAsync(string? url, int? userId)
        {
            return await _context.UrlReports.Where(x => x.Url.Equals(url) && 
                                                    x.UserId.Equals(userId)).
                                                    FirstOrDefaultAsync();
        }

        public async Task<List<UrlReport>> GetAllUrlReportsOfUserAsync(int id)
        {
            return await _context.UrlReports.Where(x => x.UserId.Equals(id)).ToListAsync();
        }
    }
}
