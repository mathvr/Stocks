using System.ComponentModel.DataAnnotations;
using STOCKS;

namespace stocks.Data.Entities;

public class StockOverview
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Symbol { get; set; } = null!;
    
    public string Exchange { get; set; } = null!;

    public string? Description { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public DateTimeOffset? CreatedOn { get; set; }
    
    public DateTimeOffset? ModifiedOn { get; set; }

    public virtual ICollection<StockHistory> Stockhistories { get; set; } = new List<StockHistory>();
    
    public virtual IEnumerable<Appuser> Appusers { get; set; } 
}
