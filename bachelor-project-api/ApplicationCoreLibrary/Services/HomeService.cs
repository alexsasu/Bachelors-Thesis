using ApplicationCoreLibrary.DTOs;
using ApplicationCoreLibrary.Entities;
using ApplicationCoreLibrary.Exceptions;
using ApplicationCoreLibrary.Helpers;
using ApplicationCoreLibrary.Interfaces.Repositories;
using ApplicationCoreLibrary.Interfaces.Services;

namespace ApplicationCoreLibrary.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _repository;
        private readonly IUrlAnalysisService _urlAnalysisService;

        public HomeService(IUnitOfWork repository,
                            IUrlAnalysisService urlAnalysisService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _urlAnalysisService = urlAnalysisService ?? throw new ArgumentNullException(nameof(urlAnalysisService));
        }

        public UrlAnalysis GetUrlAnalysis(string? url)
        {
            if (url == null)
            {
                throw new InvalidUrlAnalysisGetRequestException();
            }

            UrlAnalysis urlAnalysis = _urlAnalysisService.GenerateUrlAnalysis(url);

            return urlAnalysis;
        }

        public async Task<UrlAnalysis> AddOrUpdateUrlReport(string? url, int? userId)
        {
            try
            {
                await GetUrlReportOfUser(url, userId);

                var urlAnalysis = await UpdateUrlReport(url, userId);
                return urlAnalysis;
            }
            catch (UrlReportOfGivenUserNotFoundException)
            {
                var urlAnalysis = await AddUrlReport(url, userId);
                return urlAnalysis;
            }
        }

        public async Task<UrlAnalysis> AddUrlReport(string? url, int? userId)
        {
            UrlAnalysis urlAnalysis = _urlAnalysisService.GenerateUrlAnalysis(url);

            var report = new UrlReport()
            {
                Url = urlAnalysis.Url,
                Status = urlAnalysis.Status,
                Domain = urlAnalysis.Domain,
                RegistrarName = urlAnalysis.RegistrarName,
                RegistrarUrl = urlAnalysis.RegistrarUrl,
                DomainCreationDate = urlAnalysis.DomainCreationDate,
                DomainExpirationDate = urlAnalysis.DomainExpirationDate,
                UserId = userId
            };

            _repository.UrlReport.Create(report);
            await _repository.UrlReport.SaveAsync();

            return urlAnalysis;
        }

        public async Task<UrlAnalysis> UpdateUrlReport(string? url, int? userId)
        {
            UrlAnalysis urlAnalysis = _urlAnalysisService.GenerateUrlAnalysis(url);

            var report = await _repository.UrlReport.GetUrlReportOfUserAsync(url, userId);

            report.Status = urlAnalysis.Status;
            report.RegistrarName = urlAnalysis.RegistrarName;
            report.RegistrarUrl = urlAnalysis.RegistrarUrl;
            report.DomainCreationDate = urlAnalysis.DomainCreationDate;
            report.DomainExpirationDate = urlAnalysis.DomainExpirationDate;

            _repository.UrlReport.Update(report);
            await _repository.UrlReport.SaveAsync();

            return urlAnalysis;
        }

        public async Task<GetUrlReportDto> GetUrlReportOfUser(string? url, int? userId)
        {
            if (url == null || userId == null)
            {
                throw new InvalidUrlReportGetRequestException();
            }

            var user = await _repository.User.GetByIdAsync(userId) ?? throw new UserWithGivenIdNotFoundException(userId);

            var report = await _repository.UrlReport.GetUrlReportOfUserAsync(url, userId) ?? throw new UrlReportOfGivenUserNotFoundException(url, userId);

            return new GetUrlReportDto(report);
        }
    }
}
