using ApplicationCoreLibrary.Entities;

namespace ApplicationCoreLibrary.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByIdWithRolesAsync(int id);
        Task<User?> GetUserByEmailWithRolesAsync(string email);
        Task<List<User>> GetAllUsersWithRolesAsync();
    }
}
