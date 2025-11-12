using ApplicationCoreLibrary.Interfaces.Repositories;

namespace InfrastructureLibrary.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjDbContext _context;

        private IUrlReportRepository _urlReportRepository;
        private IUserRepository _userRepository;
        private ISessionTokenRepository _sessionTokenRepository;

        public UnitOfWork(ProjDbContext context)
        {
            _context = context;
        }

        public IUrlReportRepository UrlReport
        {
            get
            {
                if (_urlReportRepository == null) _urlReportRepository = new UrlReportRepository(_context);
                return _urlReportRepository;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository == null) _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }

        public ISessionTokenRepository SessionToken
        {
            get
            {
                if (_sessionTokenRepository == null) _sessionTokenRepository = new SessionTokenRepository(_context);
                return _sessionTokenRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
