#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.IO;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System;
using Newtonsoft.Json;

namespace Esame.Pages.Connection
{
    public class FormSQLiteModel : PageModel
    {
        
        private readonly ConnectionContext _context;

        [BindProperty]
        public SqliteOpenConnection Input { get; set; } = default!;
        
        public string Connect { get; set; }= default!;

        public string StatusMessage { get; set; }

        public IList<SqliteOpenConnection> SqliteTable { get; set; }


        public SqliteConnection connection { get; set; }


        public FormSQLiteModel(ConnectionContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            SqliteTable = await _context.SqliteOpenConnections.ToListAsync();
            Connect = @"Data Source="+ Input.Path;
            //Console.WriteLine($"Path:{Input.Path}");

            

            
            if (TestConnectionString(Input.Path, Connect))
            {
                
                for (int i = 0; i < SqliteTable.Count; i++)
                {
                    if (SqliteTable[i].Path.Equals(Input.Path))
                    {
                        StatusMessage = "I dati sono gi� stati inseriti nel database!";
                        connection.Close();
                        return Page();
                    }
                }

                _context.SqliteOpenConnections.Add(Input);
                await _context.SaveChangesAsync();
                return RedirectToPage("../Index");
             }
            else
            {
                StatusMessage = "ConnectionString non valida!";
                return Page();

            }

        }
        
        static bool TestConnectionString(string path,string connectionString)
        {

                try
                {
                var conn = new SqliteConnection(connectionString);


                    if (!System.IO.File.Exists(path))
                    {
                        Console.WriteLine("Il db non esiste");
                        return false;
                    }
                    
                    
                    conn.Open();
                    return true;

                    /*
                    var sql = "SELECT name FROM sqlite_master WHERE type='table'";
                    //string sql = "SELECT * FROM Courses";
                    using var cmd = new SqliteCommand(sql, conn);
                    using SqliteDataReader rdr=cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        Console.WriteLine("Tabella: "+ rdr.GetString(0));
                    }
                    return true;
                    */

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    return false;
                }
                    
        }

        /*
        public void QueryTest()
        {
            for (int i = 0; i < SqliteTable.Count; i++)
            {
                SqliteConnection o = JsonConvert.DeserializeObject<SqliteConnection>(SqliteTable[i].Connection);
                var sql = "SELECT name FROM sqlite_master WHERE type='table'";
                
                using var cmd = new SqliteCommand(sql, o);
                using SqliteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("Tabella: " + rdr.GetString(0));
                }
                
            }
        }
        */
    }

        
 }

