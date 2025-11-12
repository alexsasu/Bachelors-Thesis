namespace ApplicationCoreLibrary.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUrlReportRepository UrlReport { get; }
        IUserRepository User { get; }
        ISessionTokenRepository SessionToken { get; }

        Task SaveAsync();
    }
}
