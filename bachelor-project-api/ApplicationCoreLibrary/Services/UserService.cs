using ApplicationCoreLibrary.Constants;
using ApplicationCoreLibrary.DTOs;
using ApplicationCoreLibrary.Entities;
using ApplicationCoreLibrary.Exceptions;
using ApplicationCoreLibrary.Interfaces.Repositories;
using ApplicationCoreLibrary.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApplicationCoreLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _repository;

        public UserService(UserManager<User> userManager,
                            SignInManager<User> signInManager,
                            IUnitOfWork repository)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(userManager));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<UserRegistrationResultDto> RegisterUser(RegisterUserDto dto)
        {
            var exists = await _userManager.FindByEmailAsync(dto.Email);
            if (exists != null)
            {
                throw new UserAlreadyRegisteredException();
            }

            var registerUser = new User();

            registerUser.Email = dto.Email;
            registerUser.FirstName = dto.FirstName;
            registerUser.LastName = dto.LastName;
            registerUser.UserName = dto.Email;

            var result = await _userManager.CreateAsync(registerUser, dto.Password);
            if (result.Succeeded)
            {
                // await _userManager.AddToRoleAsync(registerUser, UserRoleType.Admin);
                await _userManager.AddToRoleAsync(registerUser, UserRoleType.User);

                return new UserRegistrationResultDto("User successfully registered.");
            }

            throw new FailedToRegisterUserException();
        }

        public async Task<UserLogInResultDto> LoginUser(LoginUserDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

            User? user = await _userManager.FindByEmailAsync(dto.Email);

            if (result.Succeeded && user != null)
            {
                user = await _repository.User.GetUserByIdWithRolesAsync(user.Id);

                List<string> roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

                var newJti = Guid.NewGuid().ToString();

                var tokenHandler = new JwtSecurityTokenHandler();
                var signinkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret key for auth"));

                var token = GenerateJwtToken(signinkey, user, roles, tokenHandler, newJti);

                _repository.SessionToken.Create(new SessionToken(newJti, user.Id, token.ValidTo));
                await _repository.SaveAsync();

                var tokenToReturn = tokenHandler.WriteToken(token);

                return new UserLogInResultDto("Login successful.", tokenToReturn);
            }

            throw new HadTroubleProcessingLoginCredentialsException();
        }

        private SecurityToken GenerateJwtToken(SymmetricSecurityKey signinKey, User user, List<string> roles, JwtSecurityTokenHandler tokenHandler, string jti)
        {
            var subject = new ClaimsIdentity(new Claim[]{
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            });

            foreach (var role in roles)
            {
                subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }

        public async Task<GetUserDto> GetUserByIdWithRoles(int id)
        {
            var user = await _repository.User.GetUserByIdWithRolesAsync(id) ?? throw new UserWithGivenIdNotFoundException(id);

            return new GetUserDto(user);
        }

        public async Task<GetUserDto> GetUserByEmailWithRoles(string email)
        {
            var user = await _repository.User.GetUserByEmailWithRolesAsync(email) ?? throw new UserWithGivenEmailNotFoundException(email);

            return new GetUserDto(user);
        }

        public async Task<List<GetUserDto>> GetAllUsers()
        {
            var users = await _repository.User.GetAllUsersWithRolesAsync();

            var usersToReturn = new List<GetUserDto>();
            foreach (var user in users)
            {
                usersToReturn.Add(new GetUserDto(user));
            }

            return usersToReturn;
        }

        public async Task<UserUpdatedDto> UpdateUserById(int id, UpdateUserDto dto)
        {
            var user = await _repository.User.GetByIdAsync(id) ?? throw new UserWithGivenIdNotFoundException(id);

            user.FirstName = dto.FirstName; 
            user.LastName = dto.LastName;

            _repository.User.Update(user);
            await _repository.User.SaveAsync();

            return new UserUpdatedDto("Profile successfully updated.");
        }

        public async Task<UserDeletedDto> DeleteUserById(int id)
        {
            var user = await _repository.User.GetUserByIdWithRolesAsync(id) ?? throw new UserWithGivenIdNotFoundException(id);

            _repository.User.Delete(user);
            await _repository.User.SaveAsync();

            return new UserDeletedDto("Account successfully deleted.");
        }
    }
}
