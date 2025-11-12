using ApplicationCoreLibrary.DTOs;
using ApplicationCoreLibrary.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bachelor_project_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service ?? throw new ArgumentNullException(nameof(service));
        }


        [HttpGet("getById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByIdWithRoles([FromRoute] int id)
        {
            var user = await _userService.GetUserByIdWithRoles(id);

            return Ok(user);
        }

        [HttpGet("getByEmail/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserByEmailWithRoles([FromRoute] string email)
        {
            var user = await _userService.GetUserByEmailWithRoles(email);

            return Ok(user);
        }

        [HttpGet("getAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();

            return Ok(users);
        }

        [HttpPut("updateById/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserById([FromRoute] int id, [FromBody] UpdateUserDto dto)
        {
            var result = await _userService.UpdateUserById(id, dto);

            return Ok(result);
        }

        [HttpDelete("deleteById/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserById([FromRoute] int id)
        {
            var result = await _userService.DeleteUserById(id);

            return Ok(result);
        }
    }
}
