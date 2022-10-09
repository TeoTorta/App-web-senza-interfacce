using Esame.Data;
using Esame.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.SQLite;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;


namespace Esame.Pages.Connection
{
    public class newScriptSQLiteModel : PageModel
    {
        private readonly ConnectionContext _context;

        [BindProperty]
        public Test prova { get; set; } = default;
        public String stringa { get; set; }

        public newScriptSQLiteModel(ConnectionContext context)
        {
            _context = context;
          
        }

        [BindProperty]
        public static SqliteOpenConnection Input { get; set; }

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

        public IActionResult OnPost()
        {
            
            string connectionString = $"Data Source={Input.Path}";
            SqliteConnection o = new SqliteConnection(connectionString);
            o.Open();


            var cmd = new SqliteCommand(prova.query, o);
            var dr = cmd.ExecuteReader();
            while (dr.Read())//loop through the various columns and their info
            {
                Console.WriteLine(dr.GetString(0));
            }
            return Page();
        }
    }
}
