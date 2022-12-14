#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Esame.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ConnectionContext _context;

        public DataTable PostgresTable { get; set; } = new DataTable();

        public DataTable SqliteTable { get; set; } = new DataTable();

        public IList<PostgresOpenConnection> PostgresList { get; set; }
        public IList<SqliteOpenConnection> SqliteList { get; set; }


        [BindProperty]
        public Provider Input { get; set; }

        public IndexModel(ConnectionContext context)
        {
            _context = context;
            PostgresTable.Columns.Add("Id", typeof(long));
            PostgresTable.Columns.Add("Database", typeof(string));
            PostgresTable.Columns.Add("Tabelle", typeof(IList<string>));
            SqliteTable.Columns.Add("Id", typeof(long));
            SqliteTable.Columns.Add("Database", typeof(string));
            SqliteTable.Columns.Add("Tabelle", typeof(IList<string>));
        }

        public async Task OnGetAsync()
        {
            PostgresList = await _context.PostgresOpenConnections.ToListAsync();
            Console.WriteLine(PostgresList.Count);
            if (PostgresList.Count > 0)
            {
                GetTabellePostgres();
                ViewData["PostgresTable"] = PostgresTable;
            }

            SqliteList = await _context.SqliteOpenConnections.ToListAsync();
            if (SqliteList.Count > 0)
            {
                GetTabelleSqlite();
                ViewData["SqliteTable"] = SqliteTable;
            }

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.provider.Equals(DbProvider.SQLite))
            {
                return RedirectToPage("./Connection/FormSQLite");
            }
            else
            {
                return RedirectToPage("./Connection/FormPostgres");
            }
        }

        public void GetTabellePostgres()
        {
            for (int i = 0; i < PostgresList.Count; i++)
            {
                string connectionString = $"Host={PostgresList[i].Host}; Database={PostgresList[i].Database}; User ID={PostgresList[i].UserId}; Password={PostgresList[i].Password};";

                using NpgsqlConnection o = new NpgsqlConnection(connectionString);

                o.Open();

                IList<String> tabelle = new List<String>();

                var sql = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE'";

                using var cmd = new NpgsqlCommand(sql, o);

                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    tabelle.Add(rdr.GetString(0));
                }

                string result = Path.GetFileName(o.Database);

                PostgresTable.Rows.Add(PostgresList[i].Id, result, tabelle);
            }
        }

        public void GetTabelleSqlite()
        {
            for (int i = 0; i < SqliteList.Count; i++)
            {
                string connectionString = $"Data Source={SqliteList[i].Path}";

                SqliteConnection o = new SqliteConnection(connectionString);

                o.Open();

                IList<String> tabelle = new List<String>();

                var sql = "SELECT name FROM sqlite_master WHERE type='table'";

                using var cmd = new SqliteCommand(sql, o);

                using SqliteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    tabelle.Add(rdr.GetString(0));
                }
                string result = Path.GetFileName(o.DataSource);

                SqliteTable.Rows.Add(SqliteList[i].Id, result, tabelle);
            }
        }
    }
}