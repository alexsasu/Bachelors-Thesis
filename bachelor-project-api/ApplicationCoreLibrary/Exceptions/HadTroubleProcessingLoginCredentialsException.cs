namespace ApplicationCoreLibrary.Exceptions
{
    public class HadTroubleProcessingLoginCredentialsException : Exception
    {
        public HadTroubleProcessingLoginCredentialsException()
            : base("The server had trouble processing the login credentials.")
        { 
        }
    }
}
