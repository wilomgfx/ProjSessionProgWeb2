using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace ProjetSessionWebServ2.Models
{
    // Vous pouvez ajouter des données de profil pour l'utilisateur en ajoutant plus de propriétés à votre classe ApplicationUser ; consultez http://go.microsoft.com/fwlink/?LinkID=317594 pour en savoir davantage.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Notez qu'authenticationType doit correspondre à l'élément défini dans CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ajouter les revendications personnalisées de l’utilisateur ici
            return userIdentity;    
        }

        public virtual List<Evenement> Evenements { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ProjSessionWebServ2", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Evenement> Evenements { get; set; }
		
        public System.Data.Entity.DbSet<ProjetSessionWebServ2.Models.TypeKiosque> TypeKiosques { get; set; }
        public System.Data.Entity.DbSet<ProjetSessionWebServ2.Models.TypeSpectacle> TypeSpectacles { get; set; }

        public DbSet<TypeConference> TypeConferences { get; set; }

        public System.Data.Entity.DbSet<ProjetSessionWebServ2.Models.Salle> Salles { get; set; }

        public System.Data.Entity.DbSet<ProjetSessionWebServ2.Models.Section> Sections { get; set; }

        public System.Data.Entity.DbSet<ProjetSessionWebServ2.Models.Dimension> Dimensions { get; set; }

    }
}