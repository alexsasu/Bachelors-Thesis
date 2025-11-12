using ApplicationCoreLibrary.DTOs;
using ApplicationCoreLibrary.Exceptions;
using ApplicationCoreLibrary.Interfaces.Repositories;
using ApplicationCoreLibrary.Interfaces.Services;

namespace ApplicationCoreLibrary.Services
{
    public class UrlReportService : IUrlReportService
    {
        private readonly IUnitOfWork _repository;

        public UrlReportService(IUnitOfWork repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<GetUrlReportDto> GetUrlReportById(int id)
        {
            var report = await _repository.UrlReport.GetByIdAsync(id) ?? throw new UrlReportWithGivenIdNotFoundException(id);

            return new GetUrlReportDto(report);
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

        public async Task<List<GetUrlReportDto>> GetAllUrlReportsOfUser(int id)
        {
            var user = await _repository.User.GetByIdAsync(id) ?? throw new UserWithGivenIdNotFoundException(id);

            var reports = await _repository.UrlReport.GetAllUrlReportsOfUserAsync(id);
            reports.Sort((x, y) => (y.DateModified).CompareTo(x.DateModified));

            var reportsToReturn = new List<GetUrlReportDto>();
            foreach (var report in reports)
            {
                reportsToReturn.Add(new GetUrlReportDto(report));
            }

            return reportsToReturn;
        }

        public async Task<GetUrlReportDto> DeleteUrlReportById(int id)
        {
            var report = await _repository.UrlReport.GetByIdAsync(id) ?? throw new UrlReportWithGivenIdNotFoundException(id);

            _repository.UrlReport.Delete(report);
            await _repository.UrlReport.SaveAsync();

            return new GetUrlReportDto(report);
        }
    }
}
