using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgettoSettimanale_entity_backend.Models
{
    [Table("Ordini")]
    public partial class Ordini
    {
        public Ordini()
        {
            PizzeScelte = new HashSet<PizzeScelte>();
        }

        [Key]
        public int IdOrdine { get; set; }

        public int? IdClienti { get; set; }

        [StringLength(100)]
        public string Allergie { get; set; }

        public bool? Evaso { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataConsegna { get; set; }

        [StringLength(100)]
        public string NoteAggiunta { get; set; }

        public virtual Clienti Clienti { get; set; }

        public virtual ICollection<PizzeScelte> PizzeScelte { get; } = new HashSet<PizzeScelte>();
    }
}