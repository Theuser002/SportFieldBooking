using SportFieldBooking.Biz;
using SportFieldBooking.Biz.Model.User;
using Microsoft.AspNetCore.Mvc;

namespace SportFieldBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IRepositoryWrapper _repository;

        public UserController(IConfiguration configuration, ILogger<UserController> logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }
        /// <summary>
        /// Auth: Hung
        /// Created: 18/04/2022
        /// Endpoint tao moi mot user (async)
        /// </summary>
        /// <param name="model">Biz model cho tao moi user</param>
        /// <returns>Response, tao user thanh cong thi tra ve response kem biz model cho view user, con k thi tra ve response loi</returns>
        /// <exception cref="Exception">Khi tao user bi loi</exception>
        [HttpPost("CreateUser")]
        public async Task<IActionResult> Create(New model)
        {
            try
            {
                var item = await _repository.User.CreateAsync(model);
                return Ok(item);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 19/03/2022
        /// Endpoint lay thong tin mot user
        /// </summary>
        /// <param name="id">id cua user can la</param>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var item = await _repository.User.GetAsync(id);
                return Ok(item);
            }
            catch (Exception e)
            { 
                return BadRequest(e);
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var items = await _repository.User.GetAllAsync();
                return Ok(items);
            }catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
