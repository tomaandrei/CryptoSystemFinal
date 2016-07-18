using System.Data.Entity;

namespace CryptoSystemDissertation.Models
{
    public class CryptoDbContext : DbContext
    {
        public CryptoDbContext() : base("CryptoDbUtcn")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CryptoDbContext>(new CreateDatabaseIfNotExists<CryptoDbContext>());
            modelBuilder.Entity<UserAccount>().ToTable("UserAccounts");
            modelBuilder.Entity<ImageDetails>().ToTable("ImageDetails");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<ImageDetails> ImageDetails { get; set; }
    }
}