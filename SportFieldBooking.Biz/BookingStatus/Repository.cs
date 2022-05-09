using SportFieldBooking.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using AutoMapper;
using SportFieldBooking.Biz.Model.BookingStatus;
using SportFieldBooking.Helper.Pagination;

namespace SportFieldBooking.Biz.BookingStatus
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
        /// Created: 09/05/2022
        /// method tao moi mot status
        /// </summary>
        /// <param name="model">biz model cho tao moi mot status</param>
        /// <returns>biz model cho view mot status</returns>
        public async Task<View> CreateAsync(New model)
        {
            var newStatus = _mapper.Map<Data.Model.BookingStatus>(model);
            _dbContext.BookingStatuses.Add(newStatus);
            await _dbContext.SaveChangesAsync();

            var statusView = _mapper.Map<View>(newStatus);
            return statusView;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 09/05/2022
        /// method lay ve thong tin cua mot status
        /// </summary>
        /// <param name="id">id cua status can lay thong tin</param>
        /// <returns>biz model cho view mot status</returns>
        /// <exception cref="Exception">khi status voi id nhap vao khong ton tai</exception>
        public async Task<View> GetAsync(long id)
        {
            var bookingStatus = await _dbContext.BookingStatuses.FindAsync(id);

            if (bookingStatus != null)
            {
                var bookingStatusView = _mapper.Map<View>(bookingStatus);
                return bookingStatusView;
            }
            else
            {
                throw new Exception($"There is no kind of status with the id {id}");
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 09/05/2022
        /// method lay ve danh sach cac status co phan trang
        /// </summary>
        /// <param name="pageIndex">so thu tu trang</param>
        /// <param name="pageSize">so ban ghi trong mot trang</param>
        /// <returns></returns>
        public async Task<Page<List>> GetListsAsync(long pageIndex, int pageSize)
        {
            var statusPage = await _dbContext.BookingStatuses?.OrderBy(s => s.Id).GetPagedResult<Data.Model.BookingStatus, List>(_mapper, pageIndex, pageSize);
            return statusPage;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 09/05/2022
        /// Method xoa mot status
        /// </summary>
        /// <param name="id">id cua status can xoa</param>
        /// <returns></returns>
        /// <exception cref="Exception">khi status voi id nhap vao khong ton tai</exception>
        public async Task DeleteAsync (long id)
        {
            var status = await _dbContext.BookingStatuses.FindAsync(id);
            if (status != null)
            {
                _dbContext.BookingStatuses.Remove(status);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"There's no status with the id {id}");
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 09/05/2022
        /// Method update mot status
        /// </summary>
        /// <param name="model">biz model cho update mot status</param>
        /// <returns>biz model cho view mot status</returns>
        /// <exception cref="Exception">khi status voi id nhap vao khong ton tai</exception>
        public async Task<View> UpdateAsync(Edit model)
        {
            var oldStatus = await _dbContext.BookingStatuses.FindAsync(model.Id);
            if (oldStatus != null)
            {
                var updatedStatus = _mapper.Map(model, oldStatus);
                _dbContext.BookingStatuses.Update(updatedStatus);
                // Update thay doi vao database
                await _dbContext.SaveChangesAsync();
                var updatedStatusView = _mapper.Map<View>(updatedStatus);
                return updatedStatusView;
            }
            else
            {
                throw new Exception($"There's no status with the id {model.Id}");
            }
        }

        
    }
}
