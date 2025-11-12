using ApplicationCoreLibrary.DTOs;
using ApplicationCoreLibrary.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bachelor_project_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService ?? throw new ArgumentNullException(nameof(homeService));
        }

        [HttpGet("getUrlAnalysis")]
        [AllowAnonymous]
        public IActionResult GetUrlAnalysis([FromQuery] string? url)
        {
            var urlAnalysis = _homeService.GetUrlAnalysis(url);

            return Ok(urlAnalysis);
        }

        [HttpPut("addOrUpdateUrlReport")]
        [Authorize]
        public async Task<IActionResult> AddOrUpdateUrlReport([FromBody] RequestingUrlReportDto dto)
        {
            var urlAnalysis = await _homeService.AddOrUpdateUrlReport(dto.Url, dto.UserId);

            return Ok(urlAnalysis);
        }
    }
}
