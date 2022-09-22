using SportFieldBooking.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using AutoMapper;
using SportFieldBooking.Biz.Model.Booking;
using SportFieldBooking.Helper.DateTimeUtils;
using SportFieldBooking.Helper.Enums;
using Microsoft.EntityFrameworkCore;
using SportFieldBooking.Helper.Pagination;
using Microsoft.AspNetCore.Http;

namespace SportFieldBooking.Biz.Booking
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
        /// Created: 10/05/2022
        /// method tao moi mot booking
        /// </summary>
        /// <param name="model">biz model cho tao moi mot booking</param>
        /// <returns>biz model cho view mot booking</returns>
        /// <exception cref="Exception">cac truong hop loi khac nhau khi tao moi mot booking</exception>
        public async Task<View> CreateAsync (HttpContext httpContext, New model, long userId)
        {
            #region steps
            /* Tim user trong dbContext.Users
            Tim sportField trong dbContext.SportFields
            Tao booking
            Add booking vao Bookings
            Add user vao Booking
            Add sportField vao Booking
            Add booking vao user
            Add booking vao sportField
            Add booking vao bookingStatus*/
            #endregion
            var startHour = DateTimeUtils.TakeHourOnly(model.StartHour);
            var endHour = DateTimeUtils.TakeHourOnly(model.EndHour);
            var bookDate = model.BookDate.Date;
            var sportFieldId = model.SportFieldId;

            // Check gio dat san hop le
            if (startHour > endHour || DateTime.Now.Date > bookDate || (DateTime.Now.Date == bookDate && TimeSpan.Compare(startHour.TimeOfDay, DateTime.Now.TimeOfDay) < 0))
            {
                throw new Exception("[SportFieldBooking.Biz.Booking.Repository] Invalid booking time condition");
            }

            #region lazy loading explaination
            /*
             Khong the load entity bang findasync nhu binh thuong vi theo mac dinh ef core su dung hinh thuc lazy loading,
            khi load mot entity vi du nhu sportField tu database ve local thi se k load cac related entities cua no (o day la Bookings)
            (uncomment cac dong line quan den countFiedlBookings va countUserBooking va chay api MakeBooking va xem ket qua o console). Ta muon check
            trong cac Bookings cua sportField co booking nao trung gio dang ki san khong, thi phai load duoc het cac Bookings nay ra bang
            eager loading bang cach su dung ham Include(). Tuy nhien ham Include() tra ve Iqueryable khong su dung duoc extension method FindAsync(), vi the ta phai dunng FirsOrDefaultAsync() [23, 24] 
             */
            #endregion

            var user = await _dbContext.Users.Include(u => u.Bookings).FirstOrDefaultAsync(u => u.Id == userId); 
            var sportField = await _dbContext.SportFields.Include(f => f.Bookings).FirstOrDefaultAsync(f => f.Id == sportFieldId);
            // Check nguoi dung va san bong hop le
            if (user == null || sportField == null)
            {
                throw new Exception("[SportFieldBooking.Biz.Booking.Repository] Invalid user or sport field!");
            }

            #region comments
            //var countFieldBookings = sportField.Bookings;
            //var countUserBookings = user.Bookings;
            //Console.WriteLine(countFieldBookings.Count);
            //Console.WriteLine(countUserBookings.Count);
            #endregion

            // Co the dung FirstOrDefaultAsync hoac mot function nao tuong tu the cho nhanh hon k?
            var occupiedBookings = sportField.Bookings.Where(b => b.BookDate.Date == bookDate.Date && (TimeSpan.Compare(b.EndHour.TimeOfDay, startHour.TimeOfDay) > 0 && TimeSpan.Compare(b.StartHour.TimeOfDay, endHour.TimeOfDay) < 0)).ToList();
            //Console.WriteLine(occupiedBookings.Count);
            if (occupiedBookings.Count != 0)
            {
                throw new Exception("[SportFieldBooking.Biz.Booking.Repository] The field is being rented at ur required time that day");
            }

            // Dat status cho booking moi tao la ONGOING
            var bookingStatus = await _dbContext.BookingStatuses.Where(s => s.StatusName == Consts.ONGOING_STATUS).FirstAsync();
            if (DateTimeUtils.TakeHourOnly(sportField.OpeningHour) > startHour || DateTimeUtils.TakeHourOnly(sportField.ClosingHour) < endHour)
            {
                throw new Exception("[SportFieldBooking.Biz.Booking.Repository] The field is closed at the requested time.");
            }

            var newBooking = _mapper.Map<Data.Model.Booking>(model);
            // Tinh tong thoi gian thue va gia thue tong cong
            var totalHour = (endHour - startHour).TotalHours;
            var totalPrice = (long)Math.Ceiling(totalHour) * sportField.PriceHourly;
            newBooking.TotalPrice = totalPrice;

            // Check tai khoan nguoi dung co du so du
            if (user.Balance >= totalPrice)
            {
                // Tru tien khoi tai khoan nguoi dung
                user.Balance -= totalPrice;
                // Thuc hien cac thay doi vao database
                newBooking.SportField = sportField;
                newBooking.User = user;
                newBooking.BookingStatus = bookingStatus;
                
                _dbContext.Bookings.Add(newBooking);
                await _dbContext.SaveChangesAsync();
                var bookingView = _mapper.Map<View>(newBooking);

                #region comments
                //countFieldBookings = sportField.Bookings;
                //countUserBookings = user.Bookings;
                //Console.WriteLine(countFieldBookings.Count);
                //Console.WriteLine(countUserBookings.Count);
                #endregion

                return bookingView;
            }
            else
            {
                throw new Exception("[SportFieldBooking.Biz.Booking.Repository] User's balance is not sufficient!");
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 10/05/2022
        /// method lay ve thong tin cac booking, co phan trang
        /// </summary>
        /// <param name="pageIndex">so thu tu trang</param>
        /// <param name="pageSize">so ban ghi trong mot trang</param>
        /// <returns>trang tuong ung</returns>
        public async Task<Page<List>> GetListAsync (HttpContext httpContext, long pageIndex, int pageSize)
        {
            var bookingPage = await _dbContext.Bookings?.Include(b => b.User).Include(b => b.SportField).OrderBy(b => b.Id).Include(b => b.BookingStatus).GetPagedResult<Data.Model.Booking, List>(_mapper, pageIndex, pageSize);
            return bookingPage;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 10/05/2022
        /// method xoa di mot booking nhat dinh
        /// </summary>
        /// <param name="id">id cua booking muon xoa</param>
        /// <returns></returns>
        /// <exception cref="Exception">khi khong ton tai id cua booking muon xoa</exception>
        public async Task AdminDeleteAsync (HttpContext httpContext, long bookingId)
        {
            var booking = await _dbContext.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                _dbContext.Bookings.Remove(booking);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Threre's no booking with the id {bookingId}");
            }
        }

        public async Task UserDeleteAsync (HttpContext httpContext, long userId, long bookingId)
        {
            var booking = await _dbContext.Bookings.Include(b => b.BookingStatus).FirstOrDefaultAsync(b => b.Id == bookingId && b.User.Id == userId);
            if (booking != null)
            {
                // Hoan lai tien neu booking van chua het han
                if (booking.BookingStatus.StatusName == Consts.ONGOING_STATUS)
                {
                    var total_price = booking.TotalPrice;
                    booking.User.Balance += (long)Math.Floor(total_price * Consts.REFUND_PERCENTAGE);
                }
                _dbContext.Bookings.Remove(booking);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"User {userId} has no booking with the id {bookingId}");
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 10/05/2022
        /// method lay ra cac booking cua rieng mot user, co phan trang
        /// </summary>
        /// <param name="userId">id cua user</param>
        /// <param name="pageIndex">so thu tu trang</param>
        /// <param name="pageSize">so ban ghi trong mot trang</param>
        /// <returns>trang chua cac booking cua user</returns>
        /// <exception cref="Exception">khi khong ton tai user voi id nhu vay</exception>
        public async Task<Page<List>> GetUserBooking (HttpContext httpContext, long userId, long pageIndex, int pageSize)
        {
            var bookingPage = await _dbContext.Bookings.Where(b => b.User.Id == userId).Include(b => b.User).Include(b => b.SportField).Include(b => b.BookingStatus).OrderBy(b => b.Id).GetPagedResult<Data.Model.Booking, List>(_mapper, pageIndex, pageSize);
            if (bookingPage != null)
            {
                return bookingPage;
            }
            else
            {
                throw new Exception($"The user with the id {userId} cannot be found");
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 10/05/2022
        /// method lay ra cac booking co su dung den mot san van dong nao do, co phan trang
        /// </summary>
        /// <param name="sportFieldId">id cua san van dong</param>
        /// <param name="pageIndex">so thu tu trang</param>
        /// <param name="pageSize">so ban ghi trong mot trang</param>
        /// <returns>trang chua cac booking cua san van dong do</returns>
        /// <exception cref="Exception">khi khong ton tai san bong voi id nhu vay</exception>
        public async Task<Page<List>> GetSportFieldBooking (HttpContext httpContext, long sportFieldId, long pageIndex, int pageSize)
        {
            var bookingPage = await _dbContext.Bookings.Where(b => b.SportField.Id == sportFieldId).Include(b => b.SportField).Include(b => b.User).Include(b => b.BookingStatus).OrderBy(b => b.Id).GetPagedResult<Data.Model.Booking, List>(_mapper, pageIndex, pageSize);
            if (bookingPage != null)
            {
                return bookingPage;
            }
            else
            {
                throw new Exception($"The sport field with the id {sportFieldId} cannot be found");
            }
        }

        public async Task DeactivateExpiredBookings ()
        {
            try
            {
                //Console.WriteLine("Deactivating expired bookings...");
                var outdatedBookings = await _dbContext.Bookings.Where(b => (DateTime.Compare(b.BookDate.Date, DateTime.Now.Date) < 0 || (DateTime.Compare(b.BookDate.Date, DateTime.Now.Date) == 0 && TimeSpan.Compare(b.EndHour.TimeOfDay, DateTime.Now.TimeOfDay) < 0))).ToListAsync();
                var nowDate = DateTime.Now.Date;
                var expiredStatus = await _dbContext.BookingStatuses.FirstOrDefaultAsync(s => s.StatusName == Consts.EXPIRED_STATUS);
                foreach (var booking in outdatedBookings)
                {
                    booking.BookingStatus = expiredStatus;   
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Error deactivating expired bookings");
            }
        }


    }

}
