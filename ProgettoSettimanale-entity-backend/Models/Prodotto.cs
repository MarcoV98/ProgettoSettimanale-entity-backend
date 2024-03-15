using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgettoSettimanale_entity_backend.Models
{
    public class Prodotto
    {
        public int Quantità { get; set; }

        [Key]
        public int IdPizza { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }

        [Column(TypeName = "money")]
        public decimal? Prezzo { get; set; }

        public TimeSpan? TempoConsegna { get; set; }

        public string Ingredienti { get; set; }

        public static List<Prodotto> ListPizze { get; set; } = new List<Prodotto>();

        public decimal Totale { get; set; }
    }
}