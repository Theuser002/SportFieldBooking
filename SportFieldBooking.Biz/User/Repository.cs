using SportFieldBooking.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AutoMapper;
using SportFieldBooking.Biz.Model.User;
using Microsoft.EntityFrameworkCore;


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
        /// Method tao moi mot user (async)
        /// </summary>
        /// <param name="model">Biz model cho tao moi user</param>
        /// <returns>Biz model khi view mot user</returns>
        public async Task<View> CreateAsync(New model)
        {
            // Do data tu biz model New vao data model User thong qua AutoMapper
            var itemData = _mapper.Map<Data.Model.User>(model);

            // Add data model user entity itemData vao database roi save changes
            _dbContext.Users.Add(itemData);
            await _dbContext.SaveChangesAsync();

            // Tra ve view
                // Do data tu biz model New vao biz model View thong qua AutoMapper 
            var item = _mapper.Map<View>(itemData);
                // return view
            return item;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 19/04/2022
        /// Method lay nhung thong tin ve mot user (async)
        /// </summary>
        /// <param name="id">id cua user trong database</param>
        /// <returns>Biz model cho view mot user</returns>
        /// <exception cref="Exception">Khi user voi id da nhap khong ton tai, xu ly o controller</exception>
        public async Task<View> GetAsync(long id)
        {
            var dataItem = await _dbContext.Users.FindAsync(id);    
            if (dataItem != null)
            {
                var item = _mapper.Map<View>(dataItem);
                return item;
            }
            else
            {
                // Throw expception cho controller xu ly
                throw new Exception($"There's no user with the id {id}");
            }
        }

        //public async Task<List<List>> GetListAsync()
        //{
        //    var dataItems = await _dbContext.Users.ToListAsync();
        //    var query = _dbContext.Set<User>();
        //    query.Where(x => x.Id == 1).Take(10).Skip(0);
        //    {
        //        PageNumber: 1,
        //        PageSize: 10,
        //        Total: 55,
        //        Results: []
        //    }
        //    return _mapper.Map<List<List>>(dataItems);
        //}

        public async Task<List<List>> GetAllAsync()
        {
            var dataItems = await _dbContext.Users.ToListAsync();
            return _mapper.Map<List<List>>(dataItems);
        }

        public async Task<List<List>> GetListAsync(long pageNumber, long pageSize, long total)
        {

            var dataItems = await _dbContext.Users.Where(x => x.Id == 1).Take(10).Skip(0).ToListAsync();
        }
    }
}
