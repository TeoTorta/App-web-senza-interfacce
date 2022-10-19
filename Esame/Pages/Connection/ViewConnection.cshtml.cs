#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace Esame.Pages.Connection
{
    public class ViewConnectionModel : PageModel
    {
        private readonly ConnectionContext _context;
        public ViewConnectionModel(ConnectionContext context)
        {
            _context = context;
        }

        public IList<PostgresOpenConnection> PostgresTable { get; set; }
        public IList<SqliteOpenConnection> SqliteTable{ get; set; }

        public async Task OnGetAsync()
        {
            PostgresTable = await _context.PostgresOpenConnections.ToListAsync();
            SqliteTable = await _context.SqliteOpenConnections.ToListAsync();
        }


        public void QueryTest()
        {
            for (int i = 1; i < SqliteTable.Count; i++)
            {
                SqliteConnection o = new SqliteConnection(SqliteTable[i].Path);
                o.Open();
                IList<String> tabelle = new List<String>();
                
                o.Open();

            }
        }
    }
}
