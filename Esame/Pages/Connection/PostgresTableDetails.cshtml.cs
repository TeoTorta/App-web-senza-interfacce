#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Npgsql;
using System.Data.SQLite;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Esame.Pages.Connection
{
    public class PostgresTableDetailsModel : PageModel
    {
        private readonly ConnectionContext _context;

        public DataTable PostgresTable { get; set; } = new DataTable();
        public DataTable Dati { get; set; } = new DataTable();


        public PostgresTableDetailsModel(ConnectionContext context)
        {
            _context = context;
            PostgresTable.Columns.Add("Colonna", typeof(int));
            PostgresTable.Columns.Add("Name", typeof(string));
            PostgresTable.Columns.Add("Type", typeof(string));
            PostgresTable.Columns.Add("NullorNot", typeof(string));
            PostgresTable.Columns.Add("PK", typeof(string));
        }

        [BindProperty]
        public PostgresOpenConnection Input { get; set; }

        public int contatore { get; set; }

        public IList<string> Pkey { get; set; }= new List<string>();


        public async Task<IActionResult> OnGetAsync(string name, long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Input = await _context.PostgresOpenConnections.FirstOrDefaultAsync(m => m.Id == id);

            if (Input == null)
            {
                return NotFound();
            }
            TableColumns(Input, name);

            foreach (DataRow r in PostgresTable.Rows)
            {
                foreach(var item in Pkey)
                {
                    if (r[1].Equals(item))
                    {
                        r["PK"] = "Primary Key";
                    }
                }
                contatore++;
                //Console.WriteLine("colonna: {0}\t Name: {1}\t Type: {2}\t  NullorNot: {3}\t", r[0], r[1], r[2], r[3]);
            }
            ViewData["PostgresTable"] = PostgresTable;
            TableValue(Input, name);
            ViewData["Dati"] = Dati;

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
            


            return Page();
        }

        public void TableColumns(PostgresOpenConnection Input, string name)
        {
            string connectionString = $"Host={Input.Host}; Database={Input.Database}; User ID={Input.UserId}; Password={Input.Password};";
            NpgsqlConnection o = new NpgsqlConnection(connectionString);
            o.Open();
            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT ordinal_position, column_name, data_type, is_nullable  FROM information_schema.columns WHERE table_schema = 'public' AND table_name = '{name}'", o);


            var dr = cmd.ExecuteReader();
            while (dr.Read())//loop through the various columns and their info
            {
                
                string NullorNot;
                if (dr.GetString(3).Equals("YES"))
                {
                    NullorNot = "NULL";
                }
                else
                {
                    NullorNot = "NOT NULL";
                }
                
                

                PostgresTable.Rows.Add(dr[0], dr.GetString(1), dr.GetString(2),NullorNot);
            }
            dr.Close();

            NpgsqlCommand cmd2 = new NpgsqlCommand($"select kc.column_name from information_schema.table_constraints tc, information_schema.key_column_usage kc where tc.constraint_type = 'PRIMARY KEY' and kc.table_name = tc.table_name and kc.table_schema = tc.table_schema and kc.constraint_name = tc.constraint_name AND kc.table_name = '{name}';", o);
            var dr2 = cmd2.ExecuteReader();
            while (dr2.Read())//loop through the various columns and their info
            {
                Pkey.Add(dr2.GetString(0));
                //Console.WriteLine(dr2.GetString(0));
            }

        }

        public void TableValue(PostgresOpenConnection Input, string name)
        {
            string connectionString = $"Host={Input.Host}; Database={Input.Database}; User ID={Input.UserId}; Password={Input.Password};";
            NpgsqlConnection o = new NpgsqlConnection(connectionString);
            o.Open();
            
            string query = "SELECT * FROM \""+name+"\" ";
            NpgsqlCommand cmd = new NpgsqlCommand(query, o);
            NpgsqlDataAdapter myAdapter = new NpgsqlDataAdapter(cmd);
            myAdapter.Fill(Dati);



        }
    }
}
