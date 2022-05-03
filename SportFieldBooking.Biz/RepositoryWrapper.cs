// Flow upward
// Tóm tất cả repository của từng entity một lại, tạo các repository object, inject dependencies cho chúng, tạo mapping
using SportFieldBooking.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace SportFieldBooking.Biz
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly DomainDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RepositoryWrapper> _logger;
        private readonly IMapper _mapper;

        private User.Repository? _user = null;
        private SportField.Repository _sportField = null;
        private Booking.Repository _booking = null;
        private BookingStatus.Repository _bookingStatus = null;

        // Nếu Repository object đang là null thì khởi tạo Repository object và inject các dependencies cần thiết
        public User.IRepository User => _user?? (_user = new User.Repository(_dbContext, _configuration, _logger, _mapper));
        public SportField.IRepository SportField => _sportField ?? (_sportField = new SportField.Repository(_dbContext, _configuration, _logger, _mapper));
        public Booking.IRepository Booking => _booking ?? (_booking = new Booking.Repository(_dbContext, _configuration, _logger, _mapper));
        public BookingStatus.IRepository BookingStatus => _bookingStatus ?? (_bookingStatus = new BookingStatus.Repository(_dbContext, _configuration, _logger, _mapper));

        // Dependencies injection, dùng để đổ dependnecies khi tạo mới các Repository Entity
        public RepositoryWrapper (DomainDbContext context, IConfiguration configuration, ILogger<RepositoryWrapper> logger)
        {
            _dbContext = context;     
            _configuration = configuration;
            _logger = logger;   
            _mapper = Mapper();   
        }

        // Tạo bộ Mapper quy định chung cho tất cả các repository, dựa trên AutoMapperProfile
        public static IMapper Mapper()
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            return mappingConfig.CreateMapper();
        }
    }
}
