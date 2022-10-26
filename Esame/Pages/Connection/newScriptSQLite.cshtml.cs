#nullable disable
using Esame.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public DataTable SqliteTable { get; set; } = new DataTable();

        public string DBName { get; set; }

        public newScriptSQLiteModel(ConnectionContext context)
        {
            _context = context;
          
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


            return Page();
        }

        public IActionResult OnPost()
        {

            string connectionString = $"Data Source={Input.Path}";
            
            SQLiteConnection o = new(connectionString);

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
                    SqliteConnection o2 = new(connectionString);

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
            ViewData["SqliteTable"] = SqliteTable;
            Contatore = Dati.Columns.Count;
            return Page();
        }
    }
}
