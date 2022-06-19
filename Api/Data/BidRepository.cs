namespace Api.Data
{

    public interface IBidRespositoty
    {
        Task<List<BidDto>> Get(int houseId);
        Task<BidDto> Add(BidDto dto);
    }
    public class BidRepository : IBidRespositoty
    {
        private readonly HouseDbContext _Context;
        public BidRepository(HouseDbContext Context)
        {
            this._Context = Context;

        }

        public async Task<BidDto> Add(BidDto dto)
        {
           var entity = new BidEntity();
           entity.HouseId = dto.HouseId;
           entity.Bidder = dto.Bidder;
           entity.Amount = dto.Amount;
           _Context.Bids.Add(entity);
           await _Context.SaveChangesAsync();
           
           return new BidDto(entity.Id, entity.HouseId, entity.Bidder, entity.Amount);
        }

        public async Task<List<BidDto>> Get(int houseId)
        {
            return await _Context.Bids.Where(x => x.HouseId.Equals(houseId)).Select(
                x => new BidDto(
                    x.Id,
                    x.HouseId,
                    x.Bidder,
                    x.Amount
                )
            ).ToListAsync();
        }
    }
}