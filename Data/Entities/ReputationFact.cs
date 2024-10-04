using System.ComponentModel.DataAnnotations;

namespace stocks.Data.Entities;

public class ReputationFact
{
    [Key]
    public Guid Id { get; set; }
    
    public string Fact { get; set; }
    
    public virtual Reputation Reputation { get; set; } = null!;
    
    public DateTimeOffset? CreatedOn { get; set; }
    
    public DateTimeOffset? ModifiedOn { get; set; }
}