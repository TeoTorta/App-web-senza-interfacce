#nullable disable
using Esame.Pages.Connection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Data.SQLite;
using System.Xml.Linq;

namespace Esame.Pages.Connection
{
    public class SqliteTableDetailsModel : PageModel
    {

        private readonly ConnectionContext _context;

        public DataTable SqliteTable { get; set; } = new DataTable();
        public DataTable Dati { get; set; } = new DataTable();


        public SqliteTableDetailsModel(ConnectionContext context)
        {
            _context = context;
            SqliteTable.Columns.Add("Colonna", typeof(int));
            SqliteTable.Columns.Add("Name", typeof(string));
            SqliteTable.Columns.Add("Type", typeof(string));
            SqliteTable.Columns.Add("NullorNot", typeof(string));
        }

        [BindProperty]
        public SqliteOpenConnection Input { get; set; }
        public int contatore = 0;
        public List<string> nomi {get; set; }= new List<string>();


        public async Task<IActionResult> OnGetAsync(string name, long? id)
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
            TableColumns(Input, name);
            ViewData["SqliteTable"] = SqliteTable;
            
            foreach (DataRow r in SqliteTable.Rows)
            {

                //Console.WriteLine("colonna: {0}\t Name: {1}\t Type: {2}\t  NullorNot: {3}\t", r[0], r[1], r[2], r[3]);
                contatore++;
            }
            TableValue(Input, name);
            ViewData["Dati"] = Dati;
            /*
            Console.WriteLine(Dati.Rows.Count);
            foreach (DataRow myRow in Dati.Rows)
            {
                foreach (DataColumn myColumn in Dati.Columns)
                {
                    Console.Write(myRow[myColumn] + "\t");
                }
                Console.WriteLine();
            }
            */
            
            return Page();
        }

        public void TableColumns(SqliteOpenConnection Input, string name)
        {
            string connectionString = $"Data Source={Input.Path}";
            SqliteConnection o = new SqliteConnection(connectionString);
            o.Open();
            var cmd = new SqliteCommand("PRAGMA table_info(" + name + ")", o);
            var dr = cmd.ExecuteReader();
            while (dr.Read())//loop through the various columns and their info
            {
                string NullorNot;
                if (dr.GetString(3).Equals(0))
                {
                    NullorNot = "NULL";
                }
                else
                {
                    NullorNot = "NOT NULL";
                }

                SqliteTable.Rows.Add(dr.GetString(0), dr.GetString(1), dr.GetString(2), NullorNot);
            }
        }

        public void TableValue(SqliteOpenConnection Input, string name)
        {
            string connectionString = $"Data Source={Input.Path}";
            SQLiteConnection o = new SQLiteConnection(connectionString);
            o.Open();
            string query = "SELECT * FROM " + name;
            SQLiteCommand cmd = new SQLiteCommand(query,o);
            SQLiteDataAdapter myAdapter = new SQLiteDataAdapter(cmd);
            myAdapter.Fill(Dati);
           


        }
    }
}


