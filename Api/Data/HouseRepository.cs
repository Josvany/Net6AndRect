using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public interface IHouseRepository
    {
        Task<List<HouseDto>> GetAll();
        Task<HouseDetailDto?> Get(int id);
    }

    public class HouseRepository : IHouseRepository
    {
        private readonly HouseDbContext _context;

        public HouseRepository(HouseDbContext context)
        {
            this._context = context;
        }

        public async Task<List<HouseDto>> GetAll()
        {
           return await _context.Houses.Select(h => new HouseDto(h.Id, h.Address, h.Country, h.Price)).ToListAsync();
        }
        public async Task<HouseDetailDto?> Get(int id)
        {
            var res = await _context.Houses.SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (res == null)
            {
                return null;
            }
            return new HouseDetailDto(res.Id, res.Address, res.Country, res.Description, res.Price, res.Photo);
        }
    }
}