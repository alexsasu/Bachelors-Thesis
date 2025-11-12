using Microsoft.AspNetCore.Identity;

namespace ApplicationCoreLibrary.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UrlReport> UrlReports { get; set; }

        public User() : base() { }
    }
}
