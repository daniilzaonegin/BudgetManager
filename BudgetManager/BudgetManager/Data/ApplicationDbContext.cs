using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<BalanceEntry> BalanceEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BalanceEntry>().Property(e => e.Description).HasMaxLength(210);
            builder.Entity<BalanceEntry>().Property(e => e.Id).UseIdentityColumn();
            builder.Entity<BalanceEntry>().Property(e => e.Amount).HasColumnType("decimal(18,2)");
            base.OnModelCreating(builder);
        }
    }
}
