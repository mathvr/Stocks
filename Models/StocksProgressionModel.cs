namespace STOCKS.Models;

public class StockProgressionModels
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int ResultsCount { get; set; }
    public List<StockProgressionModel> Models { get; set; }
}