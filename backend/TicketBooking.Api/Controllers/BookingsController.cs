using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketBooking.Api.Services;

namespace TicketBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires JWT Token
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("reserve/{showId}/{seatId}")]
        public async Task<IActionResult> Reserve(int showId, int seatId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _bookingService.ReserveSeatAsync(showId, seatId, userId);
            
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("confirm/{bookingId}")]
        public async Task<IActionResult> Confirm(int bookingId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _bookingService.ConfirmBookingAsync(bookingId, userId);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
