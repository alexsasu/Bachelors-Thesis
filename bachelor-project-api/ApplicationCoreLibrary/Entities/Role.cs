using Microsoft.AspNetCore.Identity;

namespace ApplicationCoreLibrary.Entities
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
