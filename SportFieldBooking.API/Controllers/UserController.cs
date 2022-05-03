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
        /// <param name="model"> Biz model chua cac thong tin dau vao can thiet cho tao moi user </param>
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
                _logger.LogError($"[MyLog]: Error creating user, {e}");
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
                return NotFound(e);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 20/04/2022
        /// Endpoint lay thong tin cua user co phan trang (async)
        /// </summary>
        /// <param name="pageNumber"> So thu tu trang </param>
        /// <param name="pageSize"> So ban ghi trong mot trang </param>
        /// <returns> Response, thanh cong hoac loi </returns>
        /// <exception cref="InvalidPageException"> Khi trang khong ton tai </exception>
        /// <exception cref="Exception"> Khi lay thong tin user co phan trang bi loi </exception>
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList(long pageNumber, int pageSize)
        {
            try
            {
                var items = await _repository.User.GetListAsync(pageNumber, pageSize);
                return Ok(items);
            }
            catch(Exception e)
            {
                _logger.LogError($"[MyLog]: Error getting list of users in pages, {e}");
                return NotFound(e);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/04/2022
        /// Endpoint xoa di mot user
        /// </summary>
        /// <param name="id"> id cua user </param>
        /// <returns>Response, thanh cong hoac loi</returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _repository.User.DeleteAsync(id);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError($"[MyLog]: Error deleting user, {e}");
                return NotFound(e);
            }
        }

        /// <summary>
        /// Author: Hung
        /// Created: 25/04/2022
        /// Endpoint cap nhat thong tin mot user
        /// </summary>
        /// <param name="model"> Biz model chua cac thong tin dau vao can thiet cho update mot user </param>
        /// <returns> Response, thanh cong hoac loi </returns>
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Update(Edit model)
        {
            try
            {
                var item = await _repository.User.UpdateAsync(model);
                return Ok(item);
            }
            catch(Exception e)
            {
                _logger.LogError($"[MyLog]: Error updating user, {e}");
                return NotFound(e);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/04/2022
        /// Endpoint tim kiem user voi username chua mot chuoi nhat dinh
        /// </summary>
        /// <param name="username"> chuoi dau vao </param>
        /// <param name="pageIndex"> so ban ghi trong mot trang </param>
        /// <param name="pageSize"> so user </param>
        /// <returns></returns>
        [HttpGet("SearchUsername")]
        public async Task<IActionResult> SearchUsername(string username, long pageIndex, int pageSize)
        {
            try
            {
                var items = await _repository.User.SearchUsernameAsync(username, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error searching for users, {e}");
                return NotFound(e);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/04/2022
        /// Endpoint loc user dua vao ngay khoi tao
        /// </summary>
        /// <param name="date"> ngay duoc chon lam moc, co format: yyyy-mm-dd </param>
        /// <param name="condition"> dieu kien loc after: sau, before: truoc, equal: bang voi ngay lam moc </param>
        /// <param name="pageIndex"> so thu tu trang </param>
        /// <param name="pageSize"> so ban ghi trong mot trang </param>
        /// <returns> Cac nguoi dung duoc loc ra  </returns>
        [HttpGet("FilterByCreatedDate")]
        public async Task<IActionResult> FilterByCreatedDate(string date, string condition, long pageIndex, int pageSize)
        {
            try
            {
                var items = await _repository.User.FilterCreatedDateAsync(date, condition, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error filtering users by created date, {e}");
                return NotFound(e);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 30/04/2022
        /// Endpoint cap nhat so du cua nguoi dung
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPut("UpdateBalance")]
        public async Task<IActionResult> UpdateBalance (long id, long amount)
        {
            try
            {
                var item = await _repository.User.UpdateBalanceAsync(id, amount);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error updating the balance of user with the id {id}");
                return NotFound(e);
            }
        }

    }
}
