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
        /// </summary>
        /// <param name="model">Biz model cho tao moi user</param>
        /// <returns>Response, tao user thanh cong thi tra ve response kem biz model cho view user, con k thi tra ve response loi</returns>
        /// <exception cref="Exception">Khi tao user bi loi</exception>
        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> Create(New model)
        {
            try
            {
                var item = await _repository.User.CreateAsync(model);
                return Ok(item);
            }
            catch (Exception e)
            {
                throw new Exception("jfiwowiejoed");
                return BadRequest();
            }
        }
    }
}
