namespace ApplicationCoreLibrary.Exceptions
{
    public class InvalidUrlReportGetRequestException : Exception
    {
        public InvalidUrlReportGetRequestException()
            : base($"URL or user ID cannot be null when retrieving URL report of user.")
        { 
        }
    }
}
