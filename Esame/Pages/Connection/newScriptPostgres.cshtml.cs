#nullable disable
using Esame.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Data;

namespace Esame.Pages.Connection
{
    public class newScriptPostgresModel : PageModel
    {
        private readonly ConnectionContext _context;

        [BindProperty]
        public Query statement { get; set; } = default;
        public String stringa { get; set; }
        public string errore { get; set; }

        public DataTable PostgresTable { get; set; } = new DataTable();

        public int contatore = 0;

        public List<string> columns { get; set; } = new List<string>();
        public string messaggioOk { get; set; }
        public DataTable Dati { get; set; } = new DataTable();

        public newScriptPostgresModel(ConnectionContext context)
        {
            _context = context;

        }

        [BindProperty]
        public static PostgresOpenConnection Input { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
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

            return Page();
        }

        public IActionResult OnPost()
        {
            //Console.WriteLine(Input.Database);
            string connectionString = $"Host={Input.Host}; Database={Input.Database}; User ID={Input.UserId}; Password={Input.Password};";
            NpgsqlConnection o = new NpgsqlConnection(connectionString);

            o.Open();

            //Console.WriteLine(o.State);

            var cmd = new NpgsqlCommand(prova.query, o);

            try
            {
                NpgsqlDataAdapter myAdapter = new NpgsqlDataAdapter(cmd);
                var count = myAdapter.Fill(Dati);

                o.Close();

                if(count == 0)
                {
                    messaggioOk = "La query è andata a buon fine";
                }
                else
                {
                    if(columns.Count > 0)
                    {
                        columns.Clear();
                    }

                    NpgsqlConnection o2 = new NpgsqlConnection(connectionString);

                    o2.Open();

                    var cmd2 = new NpgsqlCommand(prova.query, o2);

                    var reader = cmd2.ExecuteReader();

                    columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                }
            }
            catch(Exception e)
            {
                errore = e.Message;
            }

            /*
            var dr = cmd.ExecuteReader();

            while (dr.Read())       //loop through the various columns and their info
            {
                Console.WriteLine(dr.GetString(0));
                Console.WriteLine(dr.GetString(1));
                Console.WriteLine(dr.GetString(2));
            }
            */

            ViewData["Dati"] = Dati;
            ViewData["PostgresTable"] = PostgresTable;
            contatore = Dati.Columns.Count;

            return Page();
        }
    }
}
