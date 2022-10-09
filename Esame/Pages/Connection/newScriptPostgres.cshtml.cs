using Esame.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Esame.Pages.Connection
{
    public class newScriptPostgresModel : PageModel
    {
        private readonly ConnectionContext _context;

        [BindProperty]
        public Test prova { get; set; } = default;
        public String stringa { get; set; }

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
            Console.WriteLine(Input.Database);
            string connectionString = $"Host={Input.Host}; Database={Input.Database}; User ID={Input.UserId}; Password={Input.Password};";
            NpgsqlConnection o = new NpgsqlConnection(connectionString);

            o.Open();

            Console.WriteLine(o.State);

            var cmd = new NpgsqlCommand(prova.query, o);
            var dr = cmd.ExecuteReader();


            while (dr.Read())//loop through the various columns and their info
            {
                Console.WriteLine(dr.GetString(0));
            }
            return Page();
        }
    }
}
