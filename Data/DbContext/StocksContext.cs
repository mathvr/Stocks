using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STOCKS;
using stocks.Data.Entities;

namespace stocks.Data.DbContext;

public class StocksContext : IdentityDbContext
{
    public StocksContext(DbContextOptions<StocksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appuser> Appusers { get; set; }

    public virtual DbSet<Connection> Connections { get; set; }

    public virtual DbSet<StockHistory> Stockhistories { get; set; }

    public virtual DbSet<StockOverview> Stockoverviews { get; set; }
    
    public virtual DbSet<Split> Splits { get; set; }
    
    public virtual DbSet<Article> Articles { get; set; }
    
    public virtual DbSet<Reputation> Reputations { get; set; }
    
    public virtual DbSet<Financials> Financials { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole { Name = "User", NormalizedName = "USER" },
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }
            );
    }
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
}
