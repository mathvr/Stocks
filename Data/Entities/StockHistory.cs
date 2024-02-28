using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using STOCKS;

namespace stocks.Data.Entities;

public class StockHistory
{
    [Key]
    public Guid Id { get; set; }

    public Guid? StockOverviewId { get; set; }

    public decimal? OpenValue { get; set; }

    public decimal? CloseValue { get; set; }
    
    public decimal? HighValue { get; set; }
    
    public decimal? LowValue { get; set; }
    
    public int? Volume { get; set; }

    public DateTimeOffset Date { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public virtual StockOverview StockOverview { get; set; } = null!;
}
