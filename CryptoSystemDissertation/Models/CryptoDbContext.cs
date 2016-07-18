using System.Data.Entity;

namespace CryptoSystemDissertation.Models
{
    public class CryptoDbContext : DbContext
    {
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<ImageDetails> ImageDetails { get; set; }
    }
}