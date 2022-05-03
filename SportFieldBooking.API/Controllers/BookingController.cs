using SportFieldBooking.Biz;
using Microsoft.AspNetCore.Mvc;
using SportFieldBooking.Biz.Model.Booking;

namespace SportFieldBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IRepositoryWrapper _repository;

        public BookingController(IConfiguration configuration, ILogger<UserController> logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        [HttpPost("MakeBooking")]
        public async Task<IActionResult> Create(New model, long userId, long sportFieldId)
        {
            try
            {
                var item = await _repository.Booking.CreateAsync(model, userId, sportFieldId);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error making new booking, {e}");
                return BadRequest(e);
            }
        }
    }
}
