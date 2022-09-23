
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PostgresOpenConnectionConfiguration : IEntityTypeConfiguration<PostgresOpenConnection>
{
    public void Configure(EntityTypeBuilder<PostgresOpenConnection> builder)
    {
        // Vincoli e proprietà.
        builder
            .HasKey(p => p.Id); // Implicito di default.

        builder
            .Property(p => p.Host)
            .HasMaxLength(100);

        builder
            .Property(p => p.Database)
            .HasMaxLength(100);
            
        
        builder
            .Property(p => p.UserId)
            .HasMaxLength(100);

        builder
           .Property(p => p.Password)
           .HasMaxLength(100);

        
        builder
          .Property(p => p.Connection)
          .HasMaxLength(1000);
        

    }
}
