using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;

namespace ProgettoSettimanale_entity_backend.Models
{
    public class ModelDbContext : DbContext
    {
        public ModelDbContext() : base("name=ModelDbContext") { }

        public virtual DbSet<Clienti> Clienti { get; set; }
        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<Pizze> Pizze { get; set; }
        public virtual DbSet<PizzeScelte> PizzeScelte { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configurazione delle relazioni tra entità
            modelBuilder.Entity<Clienti>()
                .HasMany(c => c.Ordini)
                .WithOptional(o => o.Clienti)
                .HasForeignKey(o => o.IdClienti);

            modelBuilder.Entity<Pizze>()
                .HasMany(p => p.PizzeScelte)
                .WithOptional(ps => ps.Pizze)
                .HasForeignKey(ps => ps.PizzaScelta);

            // Configurazione delle proprietà
            modelBuilder.Entity<Pizze>()
                .Property(p => p.Prezzo)
                .HasPrecision(19, 4);
        }
    }
}