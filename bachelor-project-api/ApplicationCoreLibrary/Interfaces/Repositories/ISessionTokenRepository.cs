using ApplicationCoreLibrary.Entities;

namespace ApplicationCoreLibrary.Interfaces.Repositories
{
    public interface ISessionTokenRepository : IGenericRepository<SessionToken>
    {
        Task<SessionToken> GetByJTI(string jti);
    }
}
