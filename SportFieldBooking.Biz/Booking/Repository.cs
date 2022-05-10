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

        public async Task<View> CreateAsync (New model)
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
            var userId = model.UserId;
            var sportFieldId = model.SportFieldId;

            // Check gio dat san hop le
            if (startHour > endHour || DateTime.Now.Date > bookDate)
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

        public async Task<Page<List>> GetListAsync (long pageIndex, int pageSize)
        {
            var bookingPage = await _dbContext.Bookings?.OrderBy(b => b.Id).GetPagedResult<Data.Model.Booking, List>(_mapper, pageIndex, pageSize);
            return bookingPage;
        }

        public async Task DeleteAsync (long id)
        {
            var booking = await _dbContext.Bookings.FindAsync(id);
            if (booking != null)
            {
                _dbContext.Bookings.Remove(booking);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Threre's no booking with the id {id}");
            }
        }
    }

}
