using SportFieldBooking.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AutoMapper;
using SportFieldBooking.Biz.Model.SportField;
using Microsoft.EntityFrameworkCore;
using SportFieldBooking.Helper;
using SportFieldBooking.Helper.Pagination;
using SportFieldBooking.Helper.DateTimeUtils;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Microsoft.AspNetCore.Http;

namespace SportFieldBooking.Biz.SportField
{
    public class Repository : IRepository
    {
        private readonly DomainDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IRepositoryWrapper> _logger;
        private readonly IMapper _mapper;

        public Repository(DomainDbContext context, IConfiguration configuration, ILogger<RepositoryWrapper> logger, IMapper mapper)
        {
            _dbContext = context;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/05/2022
        /// Method tao moi mot san van dong
        /// </summary>
        /// <param name="model">Biz model cho tao moi mot san van dong</param>
        /// <returns>Biz model khi view mot san van dong</returns>
        public async Task<View> CreateAsync(HttpContext httpContext, New model)
        {
            model.OpeningHour = DateTimeUtils.TakeHourOnly(model.OpeningHour);
            model.ClosingHour = DateTimeUtils.TakeHourOnly(model.ClosingHour);
            var newSportField = _mapper.Map<Data.Model.SportField>(model);
            _dbContext.SportFields?.Add(newSportField);
            await _dbContext.SaveChangesAsync();
            var sportFieldView = _mapper.Map<View>(newSportField);
            return sportFieldView;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/05/2022
        /// Method lay thong tin mot san van dong
        /// </summary>
        /// <param name="id">id cua san van dong can tim</param>
        /// <returns>Biz model the khi view mot san van dong</returns>
        /// <exception cref="Exception"></exception>
        public async Task<View> GetAsync(HttpContext httpContext, long id)
        {
            var sportField = await _dbContext.SportFields.FindAsync(id);
            if (sportField != null)
            {
                var sportFieldView = _mapper.Map<View>(sportField);
                return sportFieldView;
            }
            else
            {
                // Throw expception cho controller xu ly
                throw new Exception($"There's no sport field with the id {id}");
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/05/2022
        /// Method lay danh sach cac san van dong va co phan trang
        /// </summary>
        /// <param name="pageIndex">So thu tu trang</param>
        /// <param name="pageSize">So ban ghi trong mot trang</param>
        /// <returns>Trang tuong ung chua thong tin cua cac san van dong</returns>
        public async Task<Page<List>> GetListAsync(HttpContext httpContext, long pageIndex, int pageSize)
        {
            var sportFieldPage = await _dbContext.SportFields?.OrderBy(u => u.Id).GetPagedResult<Data.Model.SportField, List>(_mapper, pageIndex, pageSize);
            return sportFieldPage;
        }


        /// <summary>
        /// Auth: Hung
        /// Created: 25/05/2022
        /// Method lay xoa mot san van dong
        /// </summary>
        /// <param name="id">Id cua san van dong muon xoa</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteAsync(HttpContext httpContext, long id)
        {
            var sportField = await _dbContext.SportFields.FindAsync(id);
            if (sportField != null)
            {
                _dbContext.SportFields.Remove(sportField);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Throw exception cho controller xu ly
                throw new Exception($"There's no sport field with the id {id}");
            }

        }

        /// <summary>
        /// Auth: Hung
        /// Created: 26/05/2022
        /// Method thay doi thong tin (update) mot san van dong
        /// </summary>
        /// <param name="model">Biz model cho viec edit thong tin san van dong</param>
        /// <returns> Biz model cho view thong tin san van dong </returns>
        /// <exception cref="Exception">Khi khong co san bong nao voi id da nhap</exception>
        public async Task<View> UpdateAsync(HttpContext httpContext, Edit model)
        {
            model.OpeningHour = DateTimeUtils.TakeHourOnly(model.OpeningHour);
            model.ClosingHour = DateTimeUtils.TakeHourOnly(model.ClosingHour);
            var oldSportField = await _dbContext.SportFields.FindAsync(model.Id);
            if (oldSportField != null)
            {
                var updatedSportField = _mapper.Map(model, oldSportField);
                _dbContext.SportFields.Update(updatedSportField);
                await _dbContext.SaveChangesAsync();
                var updatedUserView = _mapper.Map<View>(updatedSportField);
                return updatedUserView;
            }
            else
            {
                throw new Exception($"There's no sport field with the id {model.Id}");
            }
        }


        /// <summary>
        /// Auth: Hung
        /// Created: 26/05/2022
        /// Method tra ve thong tin cac san van dong co ten chua mot chuoi nhat dinh
        /// </summary>
        /// <param name="name">Ten (hoac mot phan cua ten) cua san bong muon tim kiem</param>
        /// <param name="pageIndex">So thu tu trang</param>
        /// <param name="pageSize">So ban ghi trong mot trang</param>
        /// <returns></returns>
        public async Task<Page<List>> SearchNameAsync(HttpContext httpContext, string name, long pageIndex, int pageSize)
        {
            var matchedSportFields = await _dbContext.SportFields.Where(f => f.Name.Contains(name)).OrderBy(f => f.Id).GetPagedResult<Data.Model.SportField, List>(_mapper, pageIndex, pageSize);
            return matchedSportFields;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 26/05/2022
        /// Method tra ve danh sach cac san van dong dang mo cua o thoi diem hien tai
        /// </summary>
        /// <param name="pageIndex">So thu tu trang</param>
        /// <param name="pageSize">So ban ghi trong mot trang</param>
        /// <returns></returns>
        public async Task<Page<List>> FindOpeningAsync(HttpContext httpContext, long pageIndex, int pageSize)
        {
            var now = DateTimeUtils.TakeHourOnly(DateTime.Now);
            var matchedSportFields = await _dbContext.SportFields.Where(f => TimeSpan.Compare(f.OpeningHour.TimeOfDay, now.TimeOfDay) <= 0 && TimeSpan.Compare(f.ClosingHour.TimeOfDay, now.TimeOfDay) > 0).OrderBy(f => f.Id).GetPagedResult<Data.Model.SportField, List>(_mapper, pageIndex, pageSize);
            return matchedSportFields;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 27/05/2022
        /// Method tra ve danh sach cac san van dong mo cua vao mot khoang thoi gian nhat dinh do nguoi dung lua chon
        /// </summary>
        /// <param name="timeStart">Thoi gian bat dau</param>
        /// <param name="timeEnd">Thoi gian ket thuc</param>
        /// <param name="pageIndex">So thu tu trang</param>
        /// <param name="pageSize">So ban ghi trong mot trang</param>
        /// <returns>Trang tuong ung chua thong tin cua cac san van dong thoa man dieu kien thoi gian</returns>
        public async Task<Page<List>> FilterByTime(HttpContext httpContext, DateTime timeStart, DateTime timeEnd, long pageIndex, int pageSize)
        {
            var now = DateTimeUtils.TakeHourOnly(DateTime.Now);
            var matchedSportFields = await _dbContext.SportFields.Where(f => TimeSpan.Compare(f.OpeningHour.TimeOfDay, timeStart.TimeOfDay) <= 0 && TimeSpan.Compare(f.ClosingHour.TimeOfDay, timeEnd.TimeOfDay) > 0).OrderBy(f => f.Id).GetPagedResult<Data.Model.SportField, List>(_mapper, pageIndex, pageSize);
            return matchedSportFields;
        }
    }
}
