namespace STOCKS.Models;

public class StockProgressionModel
{
    public string Symbol { get; set; }
    public decimal Difference { get; set; }
    public decimal Percent { get; set; }
    public bool HasSplit { get; set; }
}