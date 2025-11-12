using ApplicationCoreLibrary.Constants;
using ApplicationCoreLibrary.Entities;
using Microsoft.AspNetCore.Identity;

namespace InfrastructureLibrary.Seed
{
    public class SeedDb
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ProjDbContext _context;

        public SeedDb(RoleManager<Role> roleManager, ProjDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task SeedRoles()
        {
            if (_context.Roles.Any())
            {
                return;
            }

            string[] roleNames =
            {
                UserRoleType.Admin,
                UserRoleType.User
            };

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                roleResult = await _roleManager.CreateAsync(new Role
                {
                    Name = roleName
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
