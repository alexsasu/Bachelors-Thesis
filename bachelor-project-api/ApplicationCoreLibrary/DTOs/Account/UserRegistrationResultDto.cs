namespace ApplicationCoreLibrary.DTOs
{
    public class UserRegistrationResultDto
    {
        public string Message { get; set; }

        public UserRegistrationResultDto(string message) 
        {
            Message = message;
        }
    }
}
