using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVCCore.Data;
using MVCCore.Options;
using MVCCore.Services.Abstract;

namespace MVCCore.Services.Concrete
{
    public class StatsRepo : IRepo<StatsModel>
    {
        private ApplicationDbContext _context;

        public StatsRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task CreateAsync(StatsModel entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(StatsModel id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StatsModel>> GetAll()
        {
            return await _context.Stats.ToListAsync();
        }

        public Task<StatsModel> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(StatsModel entity)
        {
            var request = await _context.Stats.FirstAsync();
            if(entity.Reviews != 0)
            request.Reviews = entity.Reviews;

            if (entity.Trophies != 0)
                request.Trophies = entity.Trophies;

            if (entity.Events != 0)
                request.Events = entity.Events;
            await _context.SaveChangesAsync();
        }
    }
}
