#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.SQLite;

namespace Esame.Pages.Connection
{
    public class SqliteTableDetailsModel : PageModel
    {

        private readonly ConnectionContext _context;

        public DataTable SqliteTable { get; set; } = new DataTable();

        public DataTable Dati { get; set; } = new DataTable();

        public DataTable References { get; set; } = new DataTable();

        public DataTable Index { get; set; } = new DataTable();

        [BindProperty]
        public SqliteOpenConnection Input { get; set; }

        public int Contatore = 0;
        public string TableName { get; set; }

        public SqliteTableDetailsModel(ConnectionContext context)
        {
            _context = context;
            SqliteTable.Columns.Add("Colonna", typeof(int));
            SqliteTable.Columns.Add("Name", typeof(string));
            SqliteTable.Columns.Add("Type", typeof(string));
            SqliteTable.Columns.Add("NullorNot", typeof(string));
            SqliteTable.Columns.Add("PK", typeof(string));
            SqliteTable.Columns.Add("FK", typeof(string));

            References.Columns.Add("From", typeof(string));
            References.Columns.Add("To", typeof(string));
            References.Columns.Add("Table", typeof(string));


            Index.Columns.Add("Seq", typeof(string));
            Index.Columns.Add("Name", typeof(string));
            Index.Columns.Add("Unique", typeof(string));
            Index.Columns.Add("Origin", typeof(string));
            Index.Columns.Add("Partial", typeof(string));
          

        }

        public async Task<IActionResult> OnGetAsync(string name, long? id)
        {
            TableName = name;
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
                Contatore++;

                foreach (DataRow rf in References.Rows)
                {
                    if (rf["From"].Equals(r["name"]))
                    {
                        r["FK"]= $"({rf["from"]}) REFERENCES {rf["To"]} ({rf["Table"]})";
                    }
                }
                
            }
            TableValue(Input, name);
            ViewData["Dati"] = Dati;

            try
            {
                Tableindex(Input, name);
            }
            catch
            {
                return Page();
            }
            
            ViewData["Index"] = Index;

            return Page();
        }

        public void TableColumns(SqliteOpenConnection Input, string name)
        {
            string connectionString = $"Data Source={Input.Path}";
            using SqliteConnection o = new SqliteConnection(connectionString);
            o.Open();
            var cmd = new SqliteCommand("PRAGMA table_info(" + name + ")", o);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
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

                int value = int.Parse(dr.GetString(5));
                
                var primaryKey = "";
                if (value>0)
                {
                    primaryKey = "Primary Key";
                }
                SqliteTable.Rows.Add(dr.GetString(0), dr.GetString(1), dr.GetString(2), NullorNot, primaryKey);
            }
            dr.Close();

            var cmd2 = new SqliteCommand("PRAGMA foreign_key_list("+name+")", o);
            var dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                References.Rows.Add(dr2.GetString(3), dr2.GetString(4), dr2.GetString(2));

            }
        }

        public void TableValue(SqliteOpenConnection Input, string name)
        {
            string connectionString = $"Data Source={Input.Path}";
            using SQLiteConnection o = new SQLiteConnection(connectionString);
            o.Open();
            string query = "SELECT * FROM " + name;
            SQLiteCommand cmd = new SQLiteCommand(query,o);
            SQLiteDataAdapter myAdapter = new SQLiteDataAdapter(cmd);
            myAdapter.Fill(Dati);
        }


        public void Tableindex(SqliteOpenConnection Input, string name)
        {

            string connectionString = $"Data Source={Input.Path}";
            SqliteConnection o = new SqliteConnection(connectionString);
            o.Open();
            List<string> columnList = new List<string>();
            foreach (DataRow r in SqliteTable.Rows)
            {
                if (r["PK"] is not (object)"" or not (object)"")
                {
                    columnList.Add((string)r["Name"]);
                }
            }

            string columnString = "(";
            for (int i = 0; i < columnList.Count; i++)
            {
                if (i == columnList.Count - 1)
                {
                    columnString = columnString + columnList[i] + ")";
                }
                else
                {
                    columnString = columnString + columnList[i] + ", ";
                }

            }

            var cmd = new SqliteCommand("DROP INDEX IF EXISTS idx", o);
            var cmd2 = new SqliteCommand("CREATE INDEX idx ON " + name + " " + columnString, o);
            
            var reader = cmd.ExecuteReader();
            var reader2 = cmd2.ExecuteReader();

            string query2 = "PRAGMA index_list(" + name + ")";
            
            var cmd3 = new SqliteCommand(query2, o);
            var rd = cmd3.ExecuteReader();

            while (rd.Read())
            {
                Index.Rows.Add(rd.GetString(0),rd.GetString(1),rd.GetString(2), rd.GetString(3), rd.GetString(4));
            }    
        }
    }
}


