using ApplicationCoreLibrary.DTOs;

namespace ApplicationCoreLibrary.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserRegistrationResultDto> RegisterUser(RegisterUserDto dto);
        Task<UserLogInResultDto> LoginUser(LoginUserDto dto);
        Task<GetUserDto> GetUserByIdWithRoles(int id);
        Task<GetUserDto> GetUserByEmailWithRoles(string email);
        Task<List<GetUserDto>> GetAllUsers();
        Task<UserUpdatedDto> UpdateUserById(int id, UpdateUserDto dto);
        Task<UserDeletedDto> DeleteUserById(int id);
    }
}
