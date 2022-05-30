using SportFieldBooking.Biz;
using Microsoft.AspNetCore.Mvc;
using SportFieldBooking.Biz.Model.Booking;
using Microsoft.AspNetCore.Http;

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

        /// <summary>
        /// Auth: Hung
        /// Created: 10/05/2022
        /// Endpoint tao moi mot booking
        /// </summary>
        /// <param name="model">biz model cho tao moi mot booking</param>
        /// <returns>response</returns>
        [HttpPost("MakeBooking")]
        public async Task<IActionResult> Create(New model)
        {
            try
            {
                var item = await _repository.Booking.CreateAsync(HttpContext, model);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error making new booking, {e}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 10/05/2022
        /// Endpoint lay ve thong tin cac booking, co phan trang
        /// </summary>
        /// <param name="pageNumber">so thu tu trang</param>
        /// <param name="pageSize">so ban ghi trong mot trang</param>
        /// <returns>response</returns>
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList(long pageNumber, int pageSize)
        {
            try
            {
                var items = await _repository.Booking.GetListAsync(HttpContext, pageNumber, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 10/05/2022
        /// Endpoint xoa di mot booking
        /// </summary>
        /// <param name="id">id cua booking</param>
        /// <returns>response</returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _repository.Booking.DeleteAsync(HttpContext, id);
                return Ok();
            }catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 10/05/2022
        /// Endpoint lay ve cac booking cua mot nguoi dung
        /// </summary>
        /// <param name="userId">id cua nguoi dung</param>
        /// <param name="pageIndex">so thu tu trang</param>
        /// <param name="pageSize">so ban ghi trong mot trang</param>
        /// <returns>response</returns>
        [HttpGet("GetUserBooking")]
        public async Task<IActionResult> GetUserBooking(long userId, long pageIndex, int pageSize)
        {
            try
            {
                var items = await _repository.Booking.GetUserBooking(HttpContext, userId, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 10/05/2022
        /// Endpoint lay ve cac booking cua mot san van dong
        /// </summary>
        /// <param name="sportFieldId">id cua san van dong</param>
        /// <param name="pageIndex">so thu tu trang</param>
        /// <param name="pageSize">so ban ghi trong mot trang</param>
        /// <returns>response</returns>
        [HttpGet("GetSportFieldBooking")]
        public async Task<IActionResult> GetSportFieldBooking(long sportFieldId, long pageIndex, int pageSize)
        {
            try
            {
                var items = await _repository.Booking.GetSportFieldBooking(HttpContext, sportFieldId, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
