using System.ComponentModel.DataAnnotations;

namespace STOCKS;

public class StockOverview
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public int Cik { get; set; }

    public string Industry { get; set; } = null!;

    public string Exchange { get; set; } = null!;

    public string? Description { get; set; }

    public string Currency { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Sector { get; set; } = null!;

    public string FiscalYearEnd { get; set; } = null!;

    public decimal? MarketCapitalization { get; set; }

    public decimal? BookValue { get; set; }

    public decimal? ProfitMargin { get; set; }

    public DateTimeOffset? LatestQuarter { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public virtual ICollection<StockHistory> Stockhistories { get; set; } 
    
    public virtual IEnumerable<Appuser> Appusers { get; set; } 
}
