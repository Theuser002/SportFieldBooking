using SportFieldBooking.Biz;
using SportFieldBooking.Biz.Model.SportField;
using Microsoft.AspNetCore.Mvc;
using SportFieldBooking.Helper.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace SportFieldBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SportFieldController : ControllerBase
    {
        // Dependencies injection
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IRepositoryWrapper _repository;

        public SportFieldController(IConfiguration configuration, ILogger<UserController> logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/05/2022
        /// Endpoint tao moi mot san van dong
        /// </summary>
        /// <param name="model"> Biz model cho tao moi mot san van dong </param>
        /// <returns> Response </returns>
        [Authorize]
        [HttpPost("CreateField")]
        public async Task<IActionResult> Create(New model)
        {
            try
            {
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }

                var newSportField = await _repository.SportField.CreateAsync(HttpContext, model);
                return Ok(newSportField);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error creating sport field, {e}");
                return BadRequest(e.Message);
            }
        }


        /// <summary>
        /// Auth: Hung
        /// Created: 25/05/2022
        /// Endpoint lay thong tin mot san van dong
        /// </summary>
        /// <param name="id"> Id cua san van dong </param>
        /// <returns> Response </returns>
        [Authorize]
        [HttpPost("Get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var item = await _repository.SportField.GetAsync(HttpContext, id);
                return Ok(item);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/05/2022
        /// Endpoint lay danh sach cac san van dong va phan trang
        /// </summary>
        /// <param name="pageNumber"> So thu tu trang </param>
        /// <param name="pageSize"> So ban ghi trong mot trang </param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList(long pageNumber, int pageSize)
        {
            try
            {
                var items = await _repository.SportField.GetListAsync(HttpContext, pageNumber, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error getting list of users in pages, {e}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/05/2022
        /// Endpoint xoa di mot san van dong
        /// </summary>
        /// <param name="id">Id cua san van dong muon xoa</param>
        /// <returns> Response </returns>
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }

                await _repository.SportField.DeleteAsync(HttpContext, id);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error deleting user, {e}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 26/05/2022
        /// Enpoint thay doi thong tin (update) mot san van dong
        /// </summary>
        /// <param name="model"> Biz model cho viec edit thong tin san van dong </param>
        /// <returns>  Response </returns>
        [Authorize]
        [HttpPut("UpdateSportField")]
        public async Task<IActionResult> Update(Edit model)
        {
            try
            {
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }

                var item = await _repository.SportField.UpdateAsync(HttpContext, model);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error updating sport field, {e}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 26/05/2022
        /// Endpoint tra ve thong tin cac san van dong co ten chua mot chuoi nhat dinh
        /// </summary>
        /// <param name="name">chuoi dau vao</param>
        /// <param name="pageIndex">so thu tu trang</param>
        /// <param name="pageSize">so ban ghi trong mot trang</param>
        /// <returns>response</returns>
        [Authorize]
        [HttpGet("SearchName")]
        public async Task<IActionResult> SearchName(string name, long pageIndex, int pageSize)
        {
            try
            {
                var items = await _repository.SportField.SearchNameAsync(HttpContext, name, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error searching for sport field with the requested name, {e}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 26/05/2022
        /// Endpoint tra ve danh sach cac san van dong dang mo cua o thoi diem hien tai
        /// </summary>
        /// <param name="pageIndex">so thu tu trang</param>
        /// <param name="pageSize">so ban ghi trong mot trang</param>
        /// <returns>response</returns>
        [Authorize]
        [HttpGet("OpenNow")]
        public async Task<IActionResult> OpenNow(long pageIndex, int pageSize)
        {
            try
            {
                var items = await _repository.SportField.FindOpeningAsync(HttpContext, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error searching for opening sport fields, {e}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Create: 26/05/2022
        /// Endpoint tra ve danh sach cac san van dong mo cua vao mot khoang thoi gian nhat dinh do nguoi dung lua chon
        /// </summary>
        /// <param name="startTimeStr"></param>
        /// <param name="endTimeStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("FilterByTime")]
        public async Task<IActionResult> FilterByTime(string startTimeStr, string endTimeStr, long pageIndex, int pageSize)
        {
            try
            {
                DateTime startTime = DateTime.Parse(startTimeStr);
                DateTime endTime = DateTime.Parse(endTimeStr);
                var items = await _repository.SportField.FilterByTime(HttpContext, startTime, endTime, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error filtering sport fields by time, {e}");
                return NotFound(e.Message);
            }
        }
    }
}
