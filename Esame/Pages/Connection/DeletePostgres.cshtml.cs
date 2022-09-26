#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Npgsql;

namespace Esame.Pages.Connection
{
    public class DeletePostgresModel : PageModel
    {
        private readonly ConnectionContext _context;

        public DeletePostgresModel(ConnectionContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PostgresOpenConnection Input { get; set; }
       

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

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Input = await _context.PostgresOpenConnections.FindAsync(id);

            if (Input != null)
            {
                /*
                string connectionString = $"Host={Input.Host}; Database={>Input.Database}; User ID={Input.UserId}; Password={Input.Password};";
                NpgsqlConnection o = new NpgsqlConnection(connectionString);
                o.Close();
                */
                _context.PostgresOpenConnections.Remove(Input);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./ViewConnection");
        }
    }
}

