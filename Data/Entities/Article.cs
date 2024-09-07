using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using STOCKS;

namespace stocks.Data.Entities;

public class Article
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid StockOverviewId { get; set; }

    [AllowNull]
    public string Author { get; set; }

    [AllowNull]
    public string Title { get; set; }
    
    [AllowNull]
    public string Description { get; set; }

    [AllowNull]
    public string Url { get; set; }
    
    public DateTime PublicationDate { get; set; }
    
    [AllowNull]
    [MaxLength(10000)]
    public string Content { get; set; }
    
    [AllowNull]
    public string SourceName { get; set; }
    
    public DateTimeOffset? CreatedOn { get; set; }
    
    public DateTimeOffset? ModifiedOn { get; set; }
    
    public virtual StockOverview StockOverview { get; set; } = null!;
}