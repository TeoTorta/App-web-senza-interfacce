#nullable disable
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql;

public class ConnectionContext: DbContext
    {
        public ConnectionContext()
        { }
        

        public ConnectionContext(DbContextOptions<ConnectionContext> options)
       : base(options)
        { }

        public DbSet<PostgresOpenConnection> PostgresOpenConnections => Set<PostgresOpenConnection>();

        public DbSet<SqliteOpenConnection> SqliteOpenConnections => Set<SqliteOpenConnection>();

    /*
        public IList<SqliteConnection> SqliteConnessioni= new List<SqliteConnection>();

        public IList<NpgsqlConnection> PostgresConnessioni= new List<NpgsqlConnection>();

    */


    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applico la configurazione per ciascuna entità.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresOpenConnectionConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteOpenConnectionConfiguration).Assembly);
           
        }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Abilito la scrittura del log.
        //optionsBuilder.LogTo(WriteLine);

        // Serve ai tool EF (Add-Migration, ...)

           
            optionsBuilder.UseSqlite(ConnectionStrings.Get(DbProvider.SQLite));

        //base.OnConfiguring(optionsBuilder);
    }

}

/*
public class ConnectionContextFactory : IDesignTimeDbContextFactory<ConnectionContext>
{
    public ConnectionContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ConnectionContext>();
        optionsBuilder.UseSqlite(@"Data Source=Connection.db");

        return new ConnectionContext(optionsBuilder.Options);
    }
}
*/
