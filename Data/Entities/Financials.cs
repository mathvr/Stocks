using System.ComponentModel.DataAnnotations;

namespace stocks.Data.Entities;

public class Financials
{
    [Key]
    public Guid Id { get; set; }
    
    public string? Symbol { get; set; }
    
    public string? Year { get; set; }
    
    public DateTimeOffset FromDate { get; set; }
    
    public DateTimeOffset ToDate { get; set; }
    
    public string Property { get; set; }
    
    public string Unit { get; set; }
    
    public string PropertyName { get; set; }
    
    public decimal Value { get; set; }
    
    public DateTimeOffset CreatedOn { get; set; }
    
    public DateTimeOffset? ModifiedOn { get; set; }
}