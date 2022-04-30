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

        public async Task<View> CreateAsync(New model)
        {
            model.OpeningHour = DateTimeUtils.TakeHourOnly(model.OpeningHour);
            model.ClosingHour = DateTimeUtils.TakeHourOnly(model.ClosingHour);
            var newSportField = _mapper.Map<Data.Model.SportField>(model);
            _dbContext.SportFields?.Add(newSportField);
            await _dbContext.SaveChangesAsync();
            var sportFieldView = _mapper.Map<View>(newSportField);
            return sportFieldView;
        }

        public async Task<View> GetAsync(long id)
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

        public async Task<Page<List>> GetListAsync(long pageIndex, int pageSize)
        {
            var sportFieldPage = await _dbContext.SportFields?.OrderBy(u => u.Id).GetPagedResult<Data.Model.SportField, List>(_mapper, pageIndex, pageSize);
            return sportFieldPage;
        }

        public async Task DeleteAsync(long id)
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

        public async Task<View> UpdateAsync(Edit model)
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

        public async Task<Page<List>> SearchNameAsync(string name, long pageIndex, int pageSize)
        {
            var matchedSportFields = await _dbContext.SportFields.Where(f => f.Name.Contains(name)).OrderBy(f => f.Id).GetPagedResult<Data.Model.SportField, List>(_mapper, pageIndex, pageSize);
            return matchedSportFields;
        }

        public async Task<Page<List>> FindOpeningAsync(long pageIndex, int pageSize)
        {
            var now = DateTimeUtils.TakeHourOnly(DateTime.Now);
            var matchedSportFields = await _dbContext.SportFields.Where(f => TimeSpan.Compare(f.OpeningHour.TimeOfDay, now.TimeOfDay) <= 0 && TimeSpan.Compare(f.ClosingHour.TimeOfDay, now.TimeOfDay) > 0).OrderBy(f => f.Id).GetPagedResult<Data.Model.SportField, List>(_mapper, pageIndex, pageSize);
            return matchedSportFields;
        }

        public async Task<Page<List>> FilterByTime(DateTime timeStart, DateTime timeEnd, long pageIndex, int pageSize)
        {
            var now = DateTimeUtils.TakeHourOnly(DateTime.Now);
            var matchedSportFields = await _dbContext.SportFields.Where(f => TimeSpan.Compare(f.OpeningHour.TimeOfDay, timeStart.TimeOfDay) <= 0 && TimeSpan.Compare(f.ClosingHour.TimeOfDay, timeEnd.TimeOfDay) > 0).OrderBy(f => f.Id).GetPagedResult<Data.Model.SportField, List>(_mapper, pageIndex, pageSize);
            return matchedSportFields;
        }
    }
}
