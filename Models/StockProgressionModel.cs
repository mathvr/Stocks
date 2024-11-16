namespace STOCKS.Models;

public class StockProgressionModel
{
    public string Name { get; set; }
    public decimal Difference { get; set; }
    public decimal Percent { get; set; }
    public bool HasSplit { get; set; }
    public decimal? CurrentValue { get; set; }
    public string? ValueDate { get; set; }
}