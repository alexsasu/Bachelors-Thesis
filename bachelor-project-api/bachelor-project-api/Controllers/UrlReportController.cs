using ApplicationCoreLibrary.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bachelor_project_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlReportController : ControllerBase
    {
        private readonly IUrlReportService _urlReportService;

        public UrlReportController(IUrlReportService urlReportService)
        {
            _urlReportService = urlReportService ?? throw new ArgumentNullException(nameof(urlReportService));
        }

        [HttpGet("getById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUrlReportById([FromRoute] int id)
        {
            var report = await _urlReportService.GetUrlReportById(id);

            return Ok(report);
        }

        [HttpGet("getByUrlAndUserId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUrlReportOfUser([FromQuery] string? url, [FromQuery] int? userId)
        {
            var report = await _urlReportService.GetUrlReportOfUser(url, userId);

            return Ok(report);
        }

        [HttpGet("getAllReportsOfUser/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAllUrlReportsOfUser([FromRoute] int id)
        {
            var reports = await _urlReportService.GetAllUrlReportsOfUser(id);

            return Ok(reports);
        }

        [HttpDelete("deleteById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUrlReportById([FromRoute] int id)
        {
            var report = await _urlReportService.DeleteUrlReportById(id);

            return Ok(report);
        }
    }
}
