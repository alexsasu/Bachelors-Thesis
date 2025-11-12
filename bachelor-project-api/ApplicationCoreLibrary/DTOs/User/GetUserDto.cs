using ApplicationCoreLibrary.Entities;

namespace ApplicationCoreLibrary.DTOs
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public List<string> Roles { get; set; }

        public GetUserDto(User user)
        {
            Id = user.Id;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Roles = new List<string>();
            foreach(var role in user.UserRoles)
            {
                Roles.Add(role.Role.Name);
            }
        }
    }
}
