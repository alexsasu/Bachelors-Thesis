namespace ApplicationCoreLibrary.DTOs
{
    public class UserUpdatedDto
    {
        public string Message { get; set; }

        public UserUpdatedDto(string message)
        {
            Message = message;
        }
    }
}
