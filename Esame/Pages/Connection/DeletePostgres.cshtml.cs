#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Esame.Pages.Connection
{
    public class DeletePostgresModel : PageModel
    {
        private readonly ConnectionContext _context;

        [BindProperty]
        public PostgresOpenConnection Input { get; set; }

        public DeletePostgresModel(ConnectionContext context)
        {
            _context = context;
        }
       

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
                _context.PostgresOpenConnections.Remove(Input);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./DeleteConnection");
        }
    }
}

