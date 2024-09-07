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
    
    public virtual DbSet<Article> Articles { get; set; }

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
        => optionsBuilder.UseSqlServer("data source=172.17.0.1,1433; initial catalog=StocksDb; user id=SA; password=MademoisellePoe01!; MultipleActiveResultSets=True; TrustServerCertificate=True;");
    
    
}
