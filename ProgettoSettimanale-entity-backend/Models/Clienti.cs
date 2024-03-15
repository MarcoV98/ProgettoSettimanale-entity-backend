using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgettoSettimanale_entity_backend.Models
{
    [Table("Clienti")]
    public partial class Clienti
    {
        public Clienti()
        {
            Ordini = new HashSet<Ordini>();
        }

        [Key]
        public int IdCliente { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string Cognome { get; set; }

        [StringLength(50)]
        public string Indirizzo { get; set; }

        public virtual ICollection<Ordini> Ordini { get; } = new HashSet<Ordini>();
    }
}