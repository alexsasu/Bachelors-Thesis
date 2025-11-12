namespace ApplicationCoreLibrary.DTOs
{
    public class UserLogInResultDto
    {
        public string Message { get; set; }
        public string Token { get; set; }

        public UserLogInResultDto(string message, string token) 
        {
            Message = message;
            Token = token;
        }
    }
}
