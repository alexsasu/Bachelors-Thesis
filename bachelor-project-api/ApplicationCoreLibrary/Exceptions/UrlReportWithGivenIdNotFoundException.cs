namespace ApplicationCoreLibrary.Exceptions
{
    public class UrlReportWithGivenIdNotFoundException : Exception
    {
        public UrlReportWithGivenIdNotFoundException(int id)
            : base($"URL report with ID {id} not found.")
        { 
        }
    }
}
