namespace STOCKS.Models.Financials;

public class FinancialDto
{
    public string? Symbol { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? Year { get; set; }
    public List<PropertyDto> Properties { get; set; }
}

public class PropertyDto
{
    public string Name { get; set; }
    public string Value { get; set; }
    
    public string Unit { get; set; }
}