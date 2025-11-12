namespace ApplicationCoreLibrary.Exceptions
{
    public class InvalidUrlAnalysisGetRequestException : Exception
    {
        public InvalidUrlAnalysisGetRequestException()
            : base("URL cannot be null when retrieving URL analysis.")
        { 
        }
    }
}
