namespace ApplicationCoreLibrary.Exceptions
{
    public class UrlReportOfGivenUserNotFoundException : Exception
    {
        public UrlReportOfGivenUserNotFoundException(string url, int? id)
            : base($"User with ID {id} does not have a URL report for the URL {url}.")
        { 
        }
    }
}
