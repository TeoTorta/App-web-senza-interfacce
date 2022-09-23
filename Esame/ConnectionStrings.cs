internal static class ConnectionStrings
{
    static readonly Dictionary<DbProvider, string> configurations = new()
    {
        [DbProvider.SQLite] = @"Data Source=Connection.db",
        [DbProvider.PostgreSQL] = "Host=localhost; Database=Connection; User ID=postgres; Password=Pa$$w0rd;",
    };

    public static string Get(DbProvider provider) => configurations[provider];
}

//TEST PROVA DE CRISTOO