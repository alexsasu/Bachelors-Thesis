using ApplicationCoreLibrary.Helpers;

namespace ApplicationCoreLibrary.Interfaces.Services
{
    public interface IUrlAnalysisService
    {
        public UrlAnalysis GenerateUrlAnalysis(string? url);
        public void ObtainUrlStatusAndDomain(UrlAnalysis urlAnalysis);
        public void LookupDomain(UrlAnalysis urlAnalysis);
    }
}
