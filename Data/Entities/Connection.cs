using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOCKS;

public class Connection
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? BaseUrl { get; set; }

    public string? ClientSecret { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }
}
