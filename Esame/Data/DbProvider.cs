
using System.ComponentModel.DataAnnotations;

public class Provider
{
    [Required]
    public DbProvider provider { get; set; } = default!;

}

public enum DbProvider
    {
        SQLite,
        PostgreSQL
    }
