#nullable disable
using Esame.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using Npgsql.Internal.TypeHandlers;
using System.Data;
using System.Data.SQLite;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;


namespace Esame.Pages.Connection
{
    public class newScriptSQLiteModel : PageModel
    {
        private readonly ConnectionContext _context;

        [BindProperty]
        public Query Statement { get; set; }
        public string Errore { get; set; }

        public string MessaggioOk{ get; set; }

        public int Contatore = 0;

        public List<string> Columns { get; set; }= new List<string>();

        [BindProperty]
        public static SqliteOpenConnection Input { get; set; }

        public DataTable Dati { get; set; } = new DataTable();

        public DataTable Table { get; set; } = new DataTable();

        public DataTable PostgresTable { get; set; } = new DataTable();

        public DataTable SqliteTable { get; set; } = new DataTable();

        public IList<PostgresOpenConnection> PostgresList { get; set; }
        public IList<SqliteOpenConnection> SqliteList { get; set; }

        public string DBName { get; set; }

        public newScriptSQLiteModel(ConnectionContext context)
        {
            _context = context;
            PostgresTable.Columns.Add("Id", typeof(long));
            PostgresTable.Columns.Add("Database", typeof(string));
            PostgresTable.Columns.Add("Tabelle", typeof(IList<string>));
            SqliteTable.Columns.Add("Id", typeof(long));
            SqliteTable.Columns.Add("Database", typeof(string));
            SqliteTable.Columns.Add("Tabelle", typeof(IList<string>));

        }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Input = await _context.SqliteOpenConnections.FirstOrDefaultAsync(m => m.Id == id);
            

            if (Input == null)
            {
                return NotFound();
            }

            string connectionString = $"Data Source={Input.Path}";

            SqliteConnection o = new(connectionString);
            o.Open();
            DBName = Path.GetFileName(o.DataSource);
            o.Close();


            PostgresList = await _context.PostgresOpenConnections.ToListAsync();
            Console.WriteLine(PostgresList.Count);
            if (PostgresList.Count > 0)
            {
                GetTabellePostgres();
                ViewData["PostgresTable"] = PostgresTable;
            }

            SqliteList = await _context.SqliteOpenConnections.ToListAsync();
            if (SqliteList.Count > 0)
            {
                GetTabelleSqlite();
                ViewData["SqliteTable"] = SqliteTable;
            }


            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            string connectionString = $"Data Source={Input.Path}";
            
            using SQLiteConnection o = new(connectionString);

            o.Open();



            var cmd = new SQLiteCommand(Statement.query, o);

            try
            {
                SQLiteDataAdapter myAdapter = new(cmd);

                var count=myAdapter.Fill(Dati);

                o.Close();

                if (count==0)
                {
                    MessaggioOk = "Query run successfully on Database";
                }
                else
                {

                    if(Columns.Count>0)
                    {
                        Columns.Clear();
                    }
                    using SqliteConnection o2 = new(connectionString);

                    o2.Open();

                    

                    var cmd2 = new SqliteCommand(Statement.query, o2);

                    var reader = cmd2.ExecuteReader();

                    Columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                }            

            }
            catch(Exception ex)
            {

                Errore = ex.Message;
            }

            ViewData["Dati"] = Dati;
            ViewData["Table"] = Table;
            Contatore = Dati.Columns.Count;

            PostgresList = await _context.PostgresOpenConnections.ToListAsync();
            if (PostgresList.Count > 0)
            {
                GetTabellePostgres();
                ViewData["PostgresTable"] = PostgresTable;
            }

            SqliteList = await _context.SqliteOpenConnections.ToListAsync();
            if (SqliteList.Count > 0)
            {
                GetTabelleSqlite();
                ViewData["SqliteTable"] = SqliteTable;
            }
            return Page();
        }
        public void GetTabellePostgres()
        {
            for (int i = 0; i < PostgresList.Count; i++)
            {
                string connectionString = $"Host={PostgresList[i].Host}; Database={PostgresList[i].Database}; User ID={PostgresList[i].UserId}; Password={PostgresList[i].Password};";

                using NpgsqlConnection o = new NpgsqlConnection(connectionString);

                o.Open();

                IList<String> tabelle = new List<String>();

                var sql = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE'";

                using var cmd = new NpgsqlCommand(sql, o);

                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    tabelle.Add(rdr.GetString(0));
                }

                string result = Path.GetFileName(o.Database);

                PostgresTable.Rows.Add(PostgresList[i].Id, result, tabelle);
            }
        }

        public void GetTabelleSqlite()
        {
            for (int i = 0; i < SqliteList.Count; i++)
            {
                string connectionString = $"Data Source={SqliteList[i].Path}";

                using SqliteConnection o = new SqliteConnection(connectionString);

                o.Open();

                IList<String> tabelle = new List<String>();

                var sql = "SELECT name FROM sqlite_master WHERE type='table'";

                using var cmd = new SqliteCommand(sql, o);

                using SqliteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    tabelle.Add(rdr.GetString(0));
                }
                string result = Path.GetFileName(o.DataSource);

                SqliteTable.Rows.Add(SqliteList[i].Id, result, tabelle);
            }
        }
    }
}

