#nullable disable
using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SqliteOpenConnections")]
public class SqliteOpenConnection
{
    public long Id { get; set; }

    [Required]
    public string Path { get; set; } = "";
    

    

}