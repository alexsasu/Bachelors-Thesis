using ApplicationCoreLibrary.Entities;
using ApplicationCoreLibrary.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLibrary.Repositories
{
    public class SessionTokenRepository : GenericRepository<SessionToken>, ISessionTokenRepository
    {
        public SessionTokenRepository(ProjDbContext context) : base(context) { }

        public async Task<SessionToken> GetByJTI(string jti)
        {
            return await _context.SessionTokens.FirstOrDefaultAsync(t => t.Jti.Equals(jti));
        }
    }
}
