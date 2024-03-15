using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgettoSettimanale_entity_backend.Models
{
    [Table("Pizze")]
    public partial class Pizze
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pizze()
        {
            PizzeScelte = new HashSet<PizzeScelte>();
        }

        [Key]
        public int IdPizza { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string Foto { get; set; }

        [Column(TypeName = "money")]
        public decimal? Prezzo { get; set; }

        public TimeSpan? TempoConsegna { get; set; }

        public string Ingredienti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PizzeScelte> PizzeScelte { get; set; }

        public static List<Pizze> ListPizze { get; set; } = new List<Pizze>();

        public static List<int> ListQuantità { get; set; } = new List<int>();
    }
}