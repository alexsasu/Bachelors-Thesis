using ApplicationCoreLibrary.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCoreLibrary.Services
{
    public static class ServicesConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IUrlAnalysisService, UrlAnalysisService>();
            services.AddScoped<IUrlReportService, UrlReportService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
