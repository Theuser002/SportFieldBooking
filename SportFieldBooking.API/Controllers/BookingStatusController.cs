using SportFieldBooking.Biz;
using Microsoft.AspNetCore.Mvc;
using SportFieldBooking.Biz.Model.BookingStatus;
using SportFieldBooking.Helper.Exceptions;

namespace SportFieldBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingStatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IRepositoryWrapper _repository;

        public BookingStatusController(IConfiguration configuration, ILogger<UserController> logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 09/05/2022
        /// endpoint tao moi mot status
        /// </summary>
        /// <param name="model">biz model cho tao moi mot status</param>
        /// <returns>response</returns>
        [HttpPost("CreateStatus")]
        public async Task<IActionResult> Create(New model)
        {
            try
            {
                var item = await _repository.BookingStatus.CreateAsync(HttpContext, model);
                return Ok(item);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Auth: hung
        /// Created: 09/05/2022
        /// endpoint lay ve thong tin mot status nhat dinh
        /// </summary>
        /// <param name="id">id cua status can lay</param>
        /// <returns>response</returns>
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var item = await _repository.BookingStatus.GetAsync(HttpContext, id);
                return Ok(item);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 09/05/2022
        /// endpoint lay ve mot trang cac status
        /// </summary>
        /// <param name="pageNumber">so thu tu trang</param>
        /// <param name="pageSize">so ban ghi trong mot trang</param>
        /// <returns></returns>
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList(long pageNumber, int pageSize)
        {
            try
            {
                var items = await _repository.BookingStatus.GetListsAsync(HttpContext, pageNumber, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 09/05/2022
        /// endpoint cho xoa mot status
        /// </summary>
        /// <param name="id">id cua status muon xoa</param>
        /// <returns>response</returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete (long id)
        {
            try
            {
                await _repository.BookingStatus.DeleteAsync(HttpContext, id);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 09/05/2022
        /// endpoint update mot status
        /// </summary>
        /// <param name="model">biz model cho update mot status</param>
        /// <returns>response</returns>
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> Update(Edit model)
        {
            try
            {
                var item = await _repository.BookingStatus.UpdateAsync(HttpContext, model);
                return Ok(item);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
