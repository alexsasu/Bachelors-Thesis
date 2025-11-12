namespace ApplicationCoreLibrary.Exceptions
{
    public class UserWithGivenIdNotFoundException : Exception
    {
        public UserWithGivenIdNotFoundException(int? id)
            : base($"No user found with ID {id}.")
        { 
        }
    }
}
