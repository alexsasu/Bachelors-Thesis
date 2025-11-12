namespace ApplicationCoreLibrary.DTOs
{
    public class UserDeletedDto
    {
        public string Message { get; set; }

        public UserDeletedDto(string message) 
        {
            Message = message;
        }
    }
}
