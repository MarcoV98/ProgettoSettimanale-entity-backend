using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgettoSettimanale_entity_backend.Models
{
    [Table("Utenti")]
    public partial class Utenti
    {
        [Key]
        public int IdUtente { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Ruolo { get; set; } = "User";
    }
}