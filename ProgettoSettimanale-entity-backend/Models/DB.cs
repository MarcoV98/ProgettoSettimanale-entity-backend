using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProgettoSettimanale_entity_backend.Models
{
    public class DB
    {
        public int IdOrdine { get; set; }
        public DateTime DataConsegna { get; set; }
        public int Quantità { get; set; }
        public int PizzaScelta { get; set; }
        public int IdPizza { get; set; }
        public int IdPizze { get; set; }
        public string Nome { get; set; }
        public string Foto { get; set; }
        public decimal? Prezzo { get; set; }
        public TimeSpan? TempoConsegna { get; set; }
        public string Ingredienti { get; set; }
        public static decimal Totale { get; set; }
        public static decimal TotaleO { get; set; }
        public static List<DB> ListIncasso = new List<DB>();


        public static void CercaPrenotazione(DateTime d)
        {
            string connection = ConfigurationManager.ConnectionStrings["ModelDbContext"]
          .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand cmd1 = new SqlCommand("select sum(Prezzo*Quantità)as Totale from Ordini inner join PizzeScelte on Ordini.IdOrdine=Pizzescelte.PizzaScelta  full join Pizze on Pizzescelte.PizzaScelta=Pizze.IdPizza where DataConsegna=@DataConsegna ", conn);
            SqlDataReader sqlreader1;
            conn.Open();
            cmd1.Parameters.AddWithValue("DataConsegna", d);
            sqlreader1 = cmd1.ExecuteReader();

            while (sqlreader1.Read())
            {
                DB c = new DB();
                DB.Totale = Convert.ToDecimal(sqlreader1["Totale"]);

                ListIncasso.Add(c);




            }
        }
        public static void SelectOrdine(int d)
        {
            string connection = ConfigurationManager.ConnectionStrings["ModelDbContext"]
          .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand cmd1 = new SqlCommand("select * from PizzeScelte full join Pizze on Pizzescelte.PizzaScelta=Pizze.IdPizza where IdOrdine=@IdOrdine", conn);
            SqlDataReader sqlreader1;
            conn.Open();
            cmd1.Parameters.AddWithValue("IdOrdine", d);
            sqlreader1 = cmd1.ExecuteReader();

            while (sqlreader1.Read())
            {
                DB c = new DB();
                c.Nome = sqlreader1["Nome"].ToString();
                c.Foto = sqlreader1["Foto"].ToString();
                c.Prezzo = Convert.ToDecimal(sqlreader1["Prezzo"]);
                c.Quantità = Convert.ToInt16(sqlreader1["Quantità"]);
                c.Ingredienti = sqlreader1["Ingredienti"].ToString();
                ListIncasso.Add(c);

            }
        }

        public static void Elimina()
        {
            string connection = ConfigurationManager.ConnectionStrings["ModelDbContext"]
            .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM PizzeScelte where IdOrdine =@id";
            cmd.Parameters.AddWithValue("id", HttpContext.Current.Request.QueryString["Id"]);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

        }
        public static void EliminaPizza()
        {
            string connection = ConfigurationManager.ConnectionStrings["ModelDbContext"]
            .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM PizzeScelte where Pizzascelta =@id";
            cmd.Parameters.AddWithValue("id", HttpContext.Current.Request.QueryString["Id"]);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

        }

    }
}