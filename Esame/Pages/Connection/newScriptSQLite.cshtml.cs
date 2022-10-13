#nullable disable
using Esame.Data;
using Esame.Migrations;
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
        public Test prova { get; set; }
        public string errore { get; set; }

        public string messaggioOk{ get; set; }

        public int contatore = 0;

        public List<string> columns { get; set; }= new List<string>();

        [BindProperty]
        public static SqliteOpenConnection Input { get; set; }

        public DataTable Dati { get; set; } = new DataTable();

        public DataTable SqliteTable { get; set; } = new DataTable();

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
            
            return Page();
        }

        public IActionResult OnPost()
        {

            string connectionString = $"Data Source={Input.Path}";
            
            SQLiteConnection o = new SQLiteConnection(connectionString);
            o.Open();
            var cmd = new SQLiteCommand(prova.query, o);
            try
            {
                SQLiteDataAdapter myAdapter = new SQLiteDataAdapter(cmd);
                var count=myAdapter.Fill(Dati);
                o.Close();
                if (count==0)
                {
                    messaggioOk = "La query è andata a buon fine";
                }
                else
                {
                    if(columns.Count>0)
                    {
                        columns.Clear();
                    }
                    SqliteConnection o2 = new SqliteConnection(connectionString);
                    o2.Open();
                    var cmd2 = new SqliteCommand(prova.query, o2);
                    var reader = cmd2.ExecuteReader();
                    columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                }            

            }
            catch(Exception ex)
            {

                errore = ex.Message;
            }


            /*
            foreach (DataRow myRow in Dati.Rows)
            {
                foreach (DataColumn myColumn in Dati.Columns)
                {
                    Console.Write(myRow[myColumn] + "\t");
                }
                Console.WriteLine();
            }
            */
            ViewData["Dati"] = Dati;
            ViewData["SqliteTable"] = SqliteTable;
            contatore = Dati.Columns.Count;
            return Page();
        }
    }
}
