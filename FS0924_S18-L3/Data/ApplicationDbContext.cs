using FS0924_S18_L3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FS0924_S18_L3.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<
            ApplicationUser,
            ApplicationRole,
            string,
            IdentityUserClaim<string>,
            ApplicationUserRole,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>,
            IdentityUserToken<string>
        >
    {
        // Costruttore che riceve le opzioni di configurazione del database e le passa alla classe base
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Definizione delle tabelle nel database tramite DbSet<>

        // Tabella per gli utenti personalizzati
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // Tabella per i ruoli personalizzati
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        // Tabella per la relazione tra utenti e ruoli
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        // Tabella per gli studenti
        public DbSet<Student> Students { get; set; }

        // Configurazione avanzata del modello durante la creazione del database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Chiama la configurazione di IdentityDbContext per garantire che tutte le impostazioni di Identity siano applicate
            base.OnModelCreating(modelBuilder);

            // Configura la relazione tra ApplicationUserRole e ApplicationUser
            modelBuilder
                .Entity<ApplicationUserRole>()
                .HasOne(ur => ur.User) // Un ApplicationUserRole ha un utente associato
                .WithMany(u => u.ApplicationUserRole) // Un utente può avere più ruoli
                .HasForeignKey(ur => ur.UserId); // Definizione della chiave esterna che collega l'utente al ruolo

            // Configura la relazione tra ApplicationUserRole e ApplicationRole
            modelBuilder
                .Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role) // Un ApplicationUserRole ha un ruolo associato
                .WithMany(u => u.ApplicationUserRole) // Un ruolo può essere assegnato a più utenti
                .HasForeignKey(ur => ur.RoleId); // Definizione della chiave esterna che collega il ruolo all'utente
        }
    }
}
