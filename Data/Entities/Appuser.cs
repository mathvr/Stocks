using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace STOCKS;

public class Appuser
{
    [Key]
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string AccessLevel { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }
    
    public virtual ICollection<StockOverview> Stockoverviews { get; set; }
}
