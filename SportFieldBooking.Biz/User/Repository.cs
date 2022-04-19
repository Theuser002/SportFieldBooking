using SportFieldBooking.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AutoMapper;
using SportFieldBooking.Biz.Model.User;


namespace SportFieldBooking.Biz.User
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
        /// Created: 18/04/2022
        /// Method tao moi mot user bat dong bo
        /// </summary>
        /// <param name="model">Biz model cho tao moi user</param>
        /// <returns>Biz model khi view mot user</returns>
        public async Task<View> CreateAsync(New model)
        {
            // Do data tu biz model New vao data model User thong qua AutoMapper
            var itemData = _mapper.Map<Data.Model.User>(model);
            _dbContext.Users.Add(itemData);
            await _dbContext.SaveChangesAsync();

            // Tra ve view
                // Do data tu biz model New vao biz model View thong qua AutoMapper 
            var item = _mapper.Map<View>(itemData);
                // return view
            return item;
        }


    }
}
