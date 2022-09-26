#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Npgsql;

namespace Esame.Pages.Connection
{
    public class PostgresTableDetailsModel : PageModel
    {
        private readonly ConnectionContext _context;

        public PostgresTableDetailsModel(ConnectionContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PostgresOpenConnection Input { get; set; }

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
            TableValue(Input, name);
            return Page();
        }

        public void TableValue(PostgresOpenConnection Input, string name)
        {
            string connectionString = $"Host={Input.Host}; Database={Input.Database}; User ID={Input.UserId}; Password={Input.Password};";
            NpgsqlConnection o = new NpgsqlConnection(connectionString);
            o.Open();
            /*
            var cmd = new NpgsqlCommand("PRAGMA table_info(" + name + ")", o);
            var dr = cmd.ExecuteReader();
            while (dr.Read())//loop through the various columns and their info
            {
                Console.WriteLine("INFO: " + dr.GetString(0) + " " + dr.GetString(1) + " " + dr.GetString(2) + " " + dr.GetString(3));
            }
            */



        }
    }
}
