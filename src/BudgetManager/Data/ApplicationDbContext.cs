using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<BalanceEntry> BalanceEntries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AuthorizedUser> AuthorizedUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BalanceEntry>().Property(e => e.Description).HasMaxLength(210);
            builder.Entity<BalanceEntry>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<BalanceEntry>().Property(e => e.Amount).HasColumnType("decimal(18,2)");

            builder.Entity<Category>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<Category>().Property(e => e.Name).HasMaxLength(60);

            base.OnModelCreating(builder);
        }
    }
}
