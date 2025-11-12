namespace ApplicationCoreLibrary.Exceptions
{
    public class UserWithGivenEmailNotFoundException : Exception
    {
        public UserWithGivenEmailNotFoundException(string? email)
            : base($"No user found with email {email}.")
        { 
        }
    }
}
