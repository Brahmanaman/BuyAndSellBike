using BuyAndSellBike.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuyAndSellBike.Data
{
    public class BuyAndSellBikeDbContext : IdentityDbContext<IdentityUser>
    {
        public BuyAndSellBikeDbContext(DbContextOptions<BuyAndSellBikeDbContext> options): base(options) { }

        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; } 
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
