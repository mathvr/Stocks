using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOCKS;

public class StockHistory
{
    [Key]
    public Guid Id { get; set; }

    public Guid StockOverviewId { get; set; }

    public decimal OpenValue { get; set; }

    public decimal? CloseValue { get; set; }

    public DateTimeOffset Date { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public virtual StockOverview StockOverview { get; set; } = null!;
}
