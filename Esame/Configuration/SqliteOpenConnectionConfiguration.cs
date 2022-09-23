using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class SqliteOpenConnectionConfiguration : IEntityTypeConfiguration<SqliteOpenConnection>
{
    public void Configure(EntityTypeBuilder<SqliteOpenConnection> builder)
    {
        // Vincoli e proprietà.
        builder
            .HasKey(p => p.Id); // Implicito di default.

        builder
            .Property(p => p.Path)
            .HasMaxLength(100);
        
        builder
         .Property(p => p.Connection)
         .HasMaxLength(1000);
        

    }
}
