using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgettoSettimanale_entity_backend.Models
{
    [Table("PizzeScelte")]
    public partial class PizzeScelte
    {
        [Key]
        public int IdPizze { get; set; }

        public int? PizzaScelta { get; set; }

        public int? Quantità { get; set; }

        public int? IdOrdine { get; set; }

        [ForeignKey("IdOrdine")]
        public virtual Ordini Ordini { get; set; }

        [ForeignKey("PizzaScelta")]
        public virtual Pizze Pizze { get; set; }
    }
}