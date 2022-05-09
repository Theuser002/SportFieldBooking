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

        public async Task<View> CreateAsync (New model, long userId, long sportFieldId)
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
            // Check gio dat san hop le
            if (startHour < endHour && DateTime.Now.Date <= bookDate)
            {
                // chay cung luc nhieu asynchronous task [17], [18]
                var user = await _dbContext.Users.FindAsync(userId);
                var sportField = await _dbContext.SportFields.FindAsync(sportFieldId);
                //await Task.WhenAll(user.AsTask(), sportField.AsTask());

                // Check nguoi dung va san bong hop le
                if (user != null && sportField != null)
                {
                    var occupiedBookings = sportField.Bookings.Where(b => DateTime.Compare(b.BookDate, bookDate) == 0 && TimeSpan.Compare(b.StartHour.TimeOfDay, startHour.TimeOfDay) <= 0 && TimeSpan.Compare(b.EndHour.TimeOfDay, endHour.TimeOfDay) >= 0);
                    if (occupiedBookings == null)
                    {
                        var bookingStatus = await _dbContext.BookingStatuses.Where(s => s.StatusName == Consts.ON_GOING_STATUS).FirstAsync();
                        if (DateTimeUtils.TakeHourOnly(sportField.OpeningHour) <= startHour && DateTimeUtils.TakeHourOnly(sportField.ClosingHour) >= endHour)
                        {
                            var newBooking = _mapper.Map<Data.Model.Booking>(model);
                            newBooking.SportField = sportField;
                            newBooking.User = user;
                            newBooking.BookingStatus = bookingStatus;

                            // Tinh tong thoi gian thue va gia thue tong cong
                            var totalHour = (endHour - startHour).TotalHours;
                            var totalPrice = (long)Math.Ceiling(totalHour) * sportField.PriceHourly;
                            newBooking.TotalPrice = totalPrice;

                            // Check tai khoan nguoi dung co du so du
                            if (user.Balance >= totalPrice)
                            {
                                // Tru tien khoi tai khoan nguoi dung
                                user.Balance -= totalPrice;
                                user.Bookings.Add(newBooking);

                                // Thuc hien cac thay doi vao database
                                await _dbContext.SaveChangesAsync();
                                var bookingView = _mapper.Map<View>(newBooking);
                                return bookingView;
                            }
                            else
                            {
                                throw new Exception("[SportFieldBooking.Biz.Booking.Repository] User's balance is not sufficient!");
                            }
                        }
                        else
                        {
                            throw new Exception("[SportFieldBooking.Biz.Booking.Repository] The field is closed at the requested time.");
                        }
                    }
                    else
                    {
                        throw new Exception("[SportFieldBooking.Biz.Booking.Repository] The field is being rented at ur required time that day");
                    }
                }
                else
                {
                    throw new Exception("[SportFieldBooking.Biz.Booking.Repository] Invalid user or sport field!");
                }
            }
            else
            {
                throw new Exception("[SportFieldBooking.Biz.Booking.Repository] Invalid booking time condition");
            }
        }

        public async Task<Page<List>> GetListAsync (long pageIndex, int pageSize)
        {
            var bookingPage = await _dbContext.Bookings?.OrderBy(b => b.Id).GetPagedResult<Data.Model.Booking, List>(_mapper, pageIndex, pageSize);
            return bookingPage;
        }
    }

}
