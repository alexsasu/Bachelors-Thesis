namespace ApplicationCoreLibrary.Exceptions
{
    public class UserAlreadyRegisteredException : Exception
    {
        public UserAlreadyRegisteredException()
            : base($"A user with the specified email is already registered.")
        { 
        }
    }
}
