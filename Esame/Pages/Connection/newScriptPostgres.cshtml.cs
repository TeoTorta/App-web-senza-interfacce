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
        public Query Statement { get; set; } = default;
        public string Errore { get; set; }

        public DataTable PostgresTable { get; set; } = new DataTable();

        public int Contatore = 0;

        public List<string> Columns { get; set; } = new List<string>();
        public string MessaggioOk { get; set; }
        public DataTable Dati { get; set; } = new DataTable();
        public string DBName { get; set; }


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

            DBName = Input.Database;

            if (Input == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {

            string connectionString = $"Host={Input.Host}; Database={Input.Database}; User ID={Input.UserId}; Password={Input.Password};";
            NpgsqlConnection o = new NpgsqlConnection(connectionString);

            o.Open();

            var cmd = new NpgsqlCommand(Statement.query, o);

            try
            {
                NpgsqlDataAdapter myAdapter = new NpgsqlDataAdapter(cmd);
                var count = myAdapter.Fill(Dati);

                o.Close();

                if(count == 0)
                {
                    MessaggioOk = "Query run successfully on Database";
                }
                else
                {
                    if(Columns.Count > 0)
                    {
                        Columns.Clear();
                    }

                    NpgsqlConnection o2 = new NpgsqlConnection(connectionString);

                    o2.Open();

                    var cmd2 = new NpgsqlCommand(Statement.query, o2);

                    var reader = cmd2.ExecuteReader();

                    Columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                }
            }
            catch(Exception e)
            {
                Errore = e.Message;
            }

            ViewData["Dati"] = Dati;
            ViewData["PostgresTable"] = PostgresTable;
            Contatore = Dati.Columns.Count;

            return Page();
        }
    }
}
