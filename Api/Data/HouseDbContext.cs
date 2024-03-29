using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class HouseDbContext : DbContext
    {

        public HouseDbContext(DbContextOptions<HouseDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<HouseEntity> Houses => Set<HouseEntity>();
        public DbSet<BidEntity> Bids => Set<BidEntity>();
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            optionsBuilder.UseSqlite($"Data Source={Path.Join(path, "houses.db")}");

            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            SeedData.Seed(builder);
        }

    }
}