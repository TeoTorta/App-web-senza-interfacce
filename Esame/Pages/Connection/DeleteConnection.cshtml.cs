#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Esame.Pages.Connection
{
    public class DeleteConnectionModel : PageModel
    {
        private readonly ConnectionContext _context;
        public DeleteConnectionModel(ConnectionContext context)
        {
            _context = context;
        }

        public IList<PostgresOpenConnection> PostgresTable { get; set; }
        public IList<SqliteOpenConnection> SqliteTable { get; set; }

        public async Task OnGetAsync()
        {
            PostgresTable = await _context.PostgresOpenConnections.ToListAsync();
            SqliteTable = await _context.SqliteOpenConnections.ToListAsync();
        }
    }
}
