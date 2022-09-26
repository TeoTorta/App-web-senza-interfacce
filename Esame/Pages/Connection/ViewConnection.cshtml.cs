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
            if (SqliteTable.Count > 0)
            {
                /*
                for (int i = 0; i < SqliteTable.Count; i++)
                {
                    Console.WriteLine(SqliteTable[i].Connection);
                    SqliteConnection o = JsonConvert.DeserializeObject<SqliteConnection>(SqliteTable[i].Connection);
                    Console.WriteLine("DATABASE= " + o.Database);
                    Console.WriteLine("STATO= " + o.State);
                    o.Open();
                    Console.WriteLine("STATO2= " + o.State);
                }
                */

                //QueryTest();
            }
        }


        public void QueryTest()
        {
            for (int i = 1; i < SqliteTable.Count; i++)
            {
                SqliteConnection o = new SqliteConnection(SqliteTable[i].Path);
                o.Open();
                IList<String> tabelle = new List<String>();
                
                o.Open();


                // INFORMAZIONI SUL TIPO DI COLONNE
                /*
                var cmd = new SqliteCommand("PRAGMA table_info("+"SqliteOpenConnections"+")", o);
                var dr = cmd.ExecuteReader();
                while (dr.Read())//loop through the various columns and their info
                {
                    Console.WriteLine("INFO: " + dr.GetString(0)+" "+ dr.GetString(1)+" "+ dr.GetString(2)+ " " + dr.GetString(3));
                }

                dr.Close();
                */

                /*INFORMAZIONI SULLA CREATE DI OGNI TABELLA
                 
                var sql= "select* from sqlite_master where type = 'table' and name = 'SqliteOpenConnections'";
                //var cmd = new SqliteCommand("PRAGMA table_info("+"SqliteOpenConnections"+") as l WHERE l.pk=1", o);
                var cmd= new SqliteCommand(sql,o);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    Console.WriteLine("INFO TABELLA: " + dr.GetString(0) + " " + dr.GetString(4));

                }

                */


                /* INFO SUL NOME DELLE TABELLE 

                var sql = "SELECT name FROM sqlite_master WHERE type='table'";
                
                using var cmd = new SqliteCommand(sql, o);
                using SqliteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                   
                   Console.WriteLine("Tabella: " + rdr.GetString(0));
                    tabelle.Add(rdr.GetString(0));
                }


                
                /*
                var sql = $"use{o.Database} SELECT * FROM {o.GetSchema()}";

                using var cmd = new SqliteCommand(sql, o);
                using SqliteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    Console.WriteLine("Tabella: " + rdr.GetString(0));
                    tabelle.Add(rdr.GetString(0));
                }
                */
                /*
                var sql1 = $"SELECT * FROM {tabelle[3]}";

                using var cmd1 = new SqliteCommand(sql1, o);
                using SqliteDataReader rdr1 = cmd1.ExecuteReader();
                while (rdr1.Read())
                {

                    Console.WriteLine(rdr.GetString(0));
                    
                }
                */






            }
        }

    }
}
