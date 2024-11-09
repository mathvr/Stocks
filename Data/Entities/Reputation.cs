using System.ComponentModel.DataAnnotations;

namespace stocks.Data.Entities;

public class Reputation
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid? StockOverviewId { get; set; }
    
    public string Symbol { get; set; }
    
    public decimal ReputationValue { get; set; }
    
    public virtual StockOverview StockOverview { get; set; } = null!;
    
    public virtual List<ReputationFact>? ReputationFacts { get; set; } = new List<ReputationFact>();
    
    public DateTimeOffset? CreatedOn { get; set; }
    
    public DateTimeOffset? ModifiedOn { get; set; }
}