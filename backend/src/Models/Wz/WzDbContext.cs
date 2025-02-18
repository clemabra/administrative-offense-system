using Microsoft.EntityFrameworkCore;

namespace src.Models.Wz
{
    public class WzDbContext(DbContextOptions<WzDbContext> options) : DbContext(options)
    {
        public required DbSet<DtoWzEntity> WzEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // automatically adds RecordId to record when added to db
            modelBuilder.Entity<DtoWzEntity>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<DtoWzEntity>()
                .Property(e => e.RowVersion)
                .IsConcurrencyToken(false);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) 
                optionsBuilder.UseSqlite("Data Source = ./Data/wz.db");
        }
    }
}

