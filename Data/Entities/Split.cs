using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace stocks.Data.Entities;

public class Split
{
    [Key]
    public Guid Id { get; set; }
    
    public DateTime ExecutionDate { get; set; }
    
    public decimal InitialValue { get; set; }
    public decimal NewValue { get; set; }
    
    public string Symbol { get; set; }
    public string SplitApiId { get; set; }
    
}