#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Npgsql;

namespace Esame.Pages.Connection
{
    public class DeleteSqliteModel : PageModel
    {
        private readonly ConnectionContext _context;

        public DeleteSqliteModel(ConnectionContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SqliteOpenConnection Input { get; set; }


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

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Input = await _context.SqliteOpenConnections.FindAsync(id);

            if (Input != null)
            {

                
                _context.SqliteOpenConnections.Remove(Input);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./ViewConnection");
        }
    }
}