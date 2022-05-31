using SportFieldBooking.Biz;
using SportFieldBooking.Biz.Model.User;
using Microsoft.AspNetCore.Mvc;
using SportFieldBooking.Helper.Exceptions;
using SportFieldBooking.Helper.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        [AllowAnonymous]
        /// <summary>
        /// Auth: Hung
        /// Created: 18/04/2022
        /// Endpoint tao moi mot user (async)
        /// </summary
        /// <param name="model"> Biz model chua cac thong tin dau vao can thiet cho tao moi user </param>
        /// <returns> Response, tao user thanh cong thi tra ve response kem biz model cho view user, con k thi tra ve response loi </returns>
        /// <exception cref="Exception"> Khi tao user bi loi </exception>
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(New model)
        {
            try
            {
                var item = await _repository.User.CreateAsync(HttpContext, model, Consts.USER_ROLE);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error creating user, {e}");
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin(New model)
        {
            try
            {
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }

                var item = await _repository.User.CreateAsync(HttpContext, model, Consts.ADMIN_ROLE);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error creating user, {e}");
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        /// <summary>
        /// Auth: Hung
        /// Created: 19/03/2022
        /// Endpoint lay thong tin mot user (async)
        /// </summary
        /// <param name="id"> Id cua user can lay </param>
        /// <returns> Response, thanh cong hoac loi </returns>
        /// <exception cref="Exception"> Khi lay thong tin user bi loi </exception>
        [HttpGet("GetInfo/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }

                var item = await _repository.User.GetAsync(HttpContext, id);
                return Ok(item);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [Authorize]
        [HttpGet("GetSelfInfo")]
        public async Task<IActionResult> GetSelfInfo()
        {
            try
            {
                var id = await _repository.JwtAuth.GetCurrentUserIdAsync(HttpContext);
                var item = await _repository.User.GetAsync(HttpContext, id);
                return Ok(item);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [Authorize]
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
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }

                var items = await _repository.User.GetListAsync(HttpContext, pageNumber, pageSize);
                return Ok(items);
            }
            catch(Exception e)
            {
                _logger.LogError($"[MyLog]: Error getting list of users in pages, {e}");
                return NotFound(e.Message);
            }
        }

        [Authorize]
        /// <summary>
        /// Auth: Hung
        /// Created: 25/04/2022
        /// Endpoint xoa di mot user
        /// </summary>
        /// <param name="id"> id cua user </param>
        /// <returns>Response, thanh cong hoac loi</returns>
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

                await _repository.User.DeleteAsync(HttpContext, id);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError($"[MyLog]: Error deleting user, {e}");
                return NotFound(e.Message);
            }
        }

        [Authorize]
        [HttpDelete("SelfDelete")]
        public async Task<IActionResult> SelfDelete()
        {
            try
            {
                var id = await _repository.JwtAuth.GetCurrentUserIdAsync(HttpContext);
                await _repository.User.DeleteAsync(HttpContext, id);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: User {HttpContext.User.Identity.Name} unable to delete own account");
                return NotFound(e.Message);
            }
        }

        [Authorize]
        /// <summary>
        /// Author: Hung
        /// Created: 25/04/2022
        /// Endpoint cap nhat thong tin mot user
        /// </summary>
        /// <param name="model"> Biz model chua cac thong tin dau vao can thiet cho update mot user </param>
        /// <returns> Response, thanh cong hoac loi </returns>
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Update(Edit model, long id)
        {
            try
            {
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }

                var item = await _repository.User.UpdateAsync(HttpContext, model, id);
                return Ok(item);
            }
            catch(Exception e)
            {
                _logger.LogError($"[MyLog]: Error updating user, {e}");
                return NotFound(e.Message);
            }
        }

        [Authorize]
        [HttpPut("SelfUpdate")]
        public async Task<IActionResult> SelfUpdate(Edit model)
        {
            try
            {
                var id = await _repository.JwtAuth.GetCurrentUserIdAsync(HttpContext);

                var item = await _repository.User.UpdateAsync(HttpContext, model, id);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error updating user, {e}");
                return NotFound(e.Message);
            }
        }

        [Authorize]
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
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }

                var items = await _repository.User.SearchUsernameAsync(HttpContext, username, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error searching for users, {e}");
                return NotFound(e.Message);
            }
        }

        [Authorize]
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
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }

                var items = await _repository.User.FilterCreatedDateAsync(HttpContext, date, condition, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error filtering users by created date, {e}");
                return NotFound(e.Message);
            }
        }

        [Authorize]
        /// <summary>
        /// Auth: Hung
        /// Created: 30/04/2022
        /// Endpoint cap nhat so du cua nguoi dung
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPut("AddBalance")]
        public async Task<IActionResult> AddBalance (long id, long amount)
        {
            try
            {
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }

                var item = await _repository.User.UpdateBalanceAsync(HttpContext, id, amount);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error updating the balance of user with the id {id}");
                return NotFound(e.Message);
            }
        }

        [Authorize]
        [HttpPut("SelfAddBalance")]
        public async Task<IActionResult> SelfAddBalance(long amount)
        {
            try
            {
                var role = _repository.JwtAuth.GetRoleFromToken(HttpContext);
                if (role != 0)
                {
                    return Unauthorized("Only admin can use this function");
                }
                var id = await _repository.JwtAuth.GetCurrentUserIdAsync(HttpContext);
                var item = await _repository.User.UpdateBalanceAsync(HttpContext, id, amount);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error updating the balance of user with the id {await _repository.JwtAuth.GetCurrentUserIdAsync(HttpContext)}");
                return NotFound(e.Message);
            }
        }
    }
}
