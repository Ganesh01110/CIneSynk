using Microsoft.AspNetCore.Mvc;
using TicketBooking.Api.Services;

namespace TicketBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        [HttpGet]
        public async Task<IActionResult> GetShows([FromQuery] DateTime? date)
        {
            var targetDate = date ?? DateTime.Today;
            var shows = await _showService.GetShowsByDayAsync(targetDate);
            return Ok(shows);
        }

        [HttpGet("{id}/seats")]
        public async Task<IActionResult> GetSeats(int id)
        {
            var details = await _showService.GetShowDetailsAsync(id);
            if (details == null) return NotFound("Show not found.");
            return Ok(details);
        }
    }
}
