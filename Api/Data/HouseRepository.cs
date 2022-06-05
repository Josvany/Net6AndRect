using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public interface IHouseRepository
    {
        Task<List<HouseDto>> GetAll();
        Task<HouseDetailDto?> Get(int id);
        Task<HouseDetailDto> Add(HouseDetailDto dto);
        Task<HouseDetailDto> Update(HouseDetailDto dto);
        Task Delete(int id);

    }

    public class HouseRepository : IHouseRepository
    {
        private readonly HouseDbContext _context;

        public HouseRepository(HouseDbContext context)
        {
            this._context = context;
        }

        private static void DtoToEntity(HouseDetailDto dto, HouseEntity e)
        {
            e.Address = dto.Address;
            e.Country = dto.Address;
            e.Description = dto.Description;
            e.Price = dto.Price;
            e.Photo = dto.Photo;
        }

        private static HouseDetailDto EntityToDetailDto(HouseEntity res)
        {
            return new HouseDetailDto(res.Id, res.Address, res.Country, res.Description, res.Price, res.Photo);
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
            return EntityToDetailDto(res);
        }

        public async Task<HouseDetailDto> Add(HouseDetailDto dto)
        {
            var entity = new HouseEntity();
            DtoToEntity(dto, entity);
            _context.Houses.Add(entity);
            await _context.SaveChangesAsync();

            return EntityToDetailDto(entity);
        }

        public async Task<HouseDetailDto> Update(HouseDetailDto dto)
        {
            var entity = await _context.Houses.FindAsync(dto.Id);

            if (entity is null)
            {
                throw new ArgumentException($"Error updating house {dto.Id}");
            }
            DtoToEntity(dto, entity);
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return EntityToDetailDto(entity);

        }

        public async Task Delete(int id)
        {
            var entity = await _context.Houses.FindAsync(id);

            if (entity is null)
            {
                throw new ArgumentException($"Error remove house {id}");
            }
            
            _context.Remove(entity);

            await _context.SaveChangesAsync();

        }
    }
}