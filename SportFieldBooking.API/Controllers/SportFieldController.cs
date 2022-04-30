using SportFieldBooking.Biz;
using SportFieldBooking.Biz.Model.SportField;
using Microsoft.AspNetCore.Mvc;
using SportFieldBooking.Helper.Exceptions;

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

        [HttpPost("CreateField")]
        public async Task<IActionResult> Create(New model)
        {
            try
            {
                var newSportField = await _repository.SportField.CreateAsync(model);
                return Ok(newSportField);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error creating sport field, {e}");
                return BadRequest(e);
            }
        }

        [HttpPost("Get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var item = await _repository.SportField.GetAsync(id);
                return Ok(item);
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList(long pageNumber, int pageSize)
        {
            try
            {
                var items = await _repository.SportField.GetListAsync(pageNumber, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error getting list of users in pages, {e}");
                return NotFound(e);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _repository.SportField.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error deleting user, {e}");
                return NotFound(e);
            }
        }

        [HttpPut("UpdateSportField")]
        public async Task<IActionResult> Update(Edit model)
        {
            try
            {
                var item = await _repository.SportField.UpdateAsync(model);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error updating sport field, {e}");
                return NotFound(e);
            }
        }

        [HttpGet("SearchName")]
        public async Task<IActionResult> SearchName(string name, long pageIndex, int pageSize)
        {
            try
            {
                var items = await _repository.SportField.SearchNameAsync(name, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error searching for sport field with the requested name, {e}");
                return NotFound(e);
            }
        }

        [HttpGet("OpenNow")]
        public async Task<IActionResult> OpenNow(long pageIndex, int pageSize)
        {
            try
            {
                var items = await _repository.SportField.FindOpeningAsync(pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error searching for opening sport fields, {e}");
                return NotFound(e);
            }
        }

        [HttpGet("FilterByTime")]
        public async Task<IActionResult> FilterByTime(string startTimeStr, string endTimeStr, long pageIndex, int pageSize)
        {
            try
            {
                DateTime startTime = DateTime.Parse(startTimeStr);
                DateTime endTime = DateTime.Parse(endTimeStr);
                var items = await _repository.SportField.FilterByTime(startTime, endTime, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error filtering sport fields by time, {e}");
                return NotFound(e);
            }
        }
    }
}
