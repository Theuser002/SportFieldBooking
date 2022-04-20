using SportFieldBooking.Biz;
using SportFieldBooking.Biz.Model.User;
using Microsoft.AspNetCore.Mvc;
using SportFieldBooking.Helper.Exceptions;

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
        /// </summary
        /// <param name="model"> Biz model cho tao moi user </param>
        /// <returns> Response, tao user thanh cong thi tra ve response kem biz model cho view user, con k thi tra ve response loi </returns>
        /// <exception cref="Exception"> Khi tao user bi loi </exception>
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
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 19/03/2022
        /// Endpoint lay thong tin mot user (async)
        /// </summary
        /// <param name="id"> Id cua user can lay </param>
        /// <returns> Response, thanh cong hoac loi </returns>
        /// <exception cref="Exception"> Khi lay thong tin user bi loi </exception>
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

        /// <summary>
        /// Auth: Hung
        /// Created: 19/04/2022
        /// Endpoint lay thong tin cua tat ca user (async)
        /// </summary>
        /// <returns> Response, thanh cong hoac loi </returns>
        /// <exception cref="Exception"> Khi lay thong tin tat ca user bi loi </exception>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _repository.User.GetAllAsync();
                return Ok(items);
            }catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 20/04/2022
        /// Endpoint lay thong tin cua user co phan trang (async)
        /// </summary>
        /// <param name="pageNumber"> So thu tu trang </param>
        /// <param name="pageSize"> So user trong mot trang </param>
        /// <param name="total"> Tong so user trong database </param>
        /// <returns> Response, thanh cong hoac loi </returns>
        /// <exception cref="InvalidPageException"> Khi trang khong ton tai </exception>
        /// <exception cref="Exception"> Khi lay thong tin user co phan trang bi loi </exception>
        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedList(long pageNumber, int pageSize, long total)
        {
            try
            {
                var items = await _repository.User.GetListAsync(pageNumber, pageSize, total);
                return Ok(items);
            }
            catch (InvalidPageException ipe)
            {
                return BadRequest(ipe);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
