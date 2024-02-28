namespace STOCKS.Models;

public class StockOverviewDto
    {
        public string Name { get; set; } = null!;

        public int Cik { get; set; }

        public string Symbol { get; set; }

        public string Industry { get; set; } = null!;

        public string Exchange { get; set; } = null!;

        public string Currency { get; set; } = null!;

        public string Sector { get; set; } = null!;

        public decimal? Marketcapitalization { get; set; }
        
        public string? Description { get; set; }
        
        public decimal? BookValue { get; set; }
        
        public decimal? ProfitMargin { get; set; }
}