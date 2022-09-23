
using Microsoft.EntityFrameworkCore;

static class DbContextOptionsBuilderExtensions
{
    
        public static DbContextOptionsBuilder<T> Use<T>(this DbContextOptionsBuilder<T> builder, DbProvider provider, string connectionString)
            where T : DbContext
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(connectionString);

            return provider switch
            {
                DbProvider.SQLite => builder.UseSqlite(connectionString),
                DbProvider.PostgreSQL => builder.UseNpgsql(connectionString),
                _ => throw new NotSupportedException(),
            };
        }
    
}
