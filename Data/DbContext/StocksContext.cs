using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace STOCKS;

public class StocksContext : DbContext
{
    public StocksContext(DbContextOptions<StocksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appuser> Appusers { get; set; }

    public virtual DbSet<Connection> Connections { get; set; }

    public virtual DbSet<StockHistory> Stockhistories { get; set; }

    public virtual DbSet<StockOverview> Stockoverviews { get; set; }
    
    /*
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=./;Initial Catalog=Stocks; Trusted_Connection=True; TrustServerCertificate=true; Integrated Security=false;");
    */
    
}
