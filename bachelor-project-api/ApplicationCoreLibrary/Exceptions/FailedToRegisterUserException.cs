namespace ApplicationCoreLibrary.Exceptions
{
    public class FailedToRegisterUserException : Exception
    {
        public FailedToRegisterUserException()
            : base("User registration failed.")
        { 
        }
    }
}
