#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using System;

namespace Esame.Pages.Connection
{
    public class FormPostgresModel : PageModel
    {
        private readonly ConnectionContext _context;

        [BindProperty]
        public PostgresOpenConnection Input { get; set; } = default!;

        public string Connect { get; set; } = default!;

        public string StatusMessage { get; set; }


        public IList<PostgresOpenConnection> PostgresTable { get; set; }

        public NpgsqlConnection connection { get; set; }



        public FormPostgresModel(ConnectionContext context)
        {
            _context = context;
        }
       

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            PostgresTable = await _context.PostgresOpenConnections.ToListAsync();
            //Console.WriteLine($"Host:{Input.Host}, database:{Input.Database}, ID:{Input.UserId}, Password:{Input.Password}");
            Connect = $"Host={Input.Host}; Database={Input.Database}; User ID={Input.UserId}; Password={Input.Password};";

            connection = TestConnectionString(Connect);

            if (connection != null)
            {
                for (int i = 0; i < PostgresTable.Count; i++)
                {
                    var item = $"Host={PostgresTable[i].Host}; Database={PostgresTable[i].Database}; User ID={PostgresTable[i].UserId}; Password={PostgresTable[i].Password};";
                    if (item.Equals(Connect))
                    {
                        StatusMessage = "I dati sono già stati inseriti nel database!";
                        return Page();
                    }
                }
                
                
                Input.Connection = JsonConvert.SerializeObject(connection);
                _context.PostgresOpenConnections.Add(Input);
                await _context.SaveChangesAsync();
                return RedirectToPage("../Index");
            }
            else
            {
                StatusMessage = "ConnectionString non valida!";
                return Page();
            }
            
        }

        static NpgsqlConnection TestConnectionString(string connectionString)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                return conn;
                    /*
                    var sql = "SELECT version()";
                    using var cmd = new NpgsqlCommand(sql, conn);
                    var version = cmd.ExecuteScalar().ToString();
                    */
                    //Console.WriteLine($"PostgreSQL version: {version}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    return null;
                }

            }
            
        }
    }

