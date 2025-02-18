using Microsoft.EntityFrameworkCore;

namespace src.Models.Offenses
{
    public class OffenseDbContext(DbContextOptions<OffenseDbContext> options) : DbContext(options)
    {
        public required DbSet<DtoOffenseRecord> OffenseRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // automatically adds RecordId to record when added to db
            modelBuilder.Entity<DtoOffenseRecord>()
                .Property(e => e.RecordId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DtoOffenseRecord>()
                .Property(e => e.RowVersion)
                .IsConcurrencyToken(false);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) 
                optionsBuilder.UseSqlite("Data Source = ./Data/offenses.db");
        }
    }
}