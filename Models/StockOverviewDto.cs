namespace STOCKS.Models;

public class StockOverviewDto
    {
        public string Name { get; set; } = null!;
        public string Symbol { get; set; }
        public string Exchange { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? ReputationValue { get; set; }
        public List<string>? ReputationFacts { get; set; } = new List<string>();
}