#nullable disable
using Microsoft.Data.Sqlite;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("PostgresOpenConnections")]

public class PostgresOpenConnection
{
    public long Id { get; set; }
    [Required]
    public string Host { get; set; } = "";

    [Required]
    public string Database { get; set; } = "";

    [Required]
    public string UserId { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
    
    public string Connection { get; set; } = default;
    

}

