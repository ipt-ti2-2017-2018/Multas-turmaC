using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Multas_tC.Models {

   // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
   /// <summary>
   /// identifica um Utilizador, dentro do sistema de autenticação Identity
   /// </summary>
   public class ApplicationUser : IdentityUser {
      public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
         // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
         var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
         // Add custom user claims here
         return userIdentity;
      }
   }


   /// <summary>
   /// especifica as características da base de dados da Autenticação,
   /// mais,
   /// as características da base de dados do 'negócio' - Multas
   /// </summary>
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
      public ApplicationDbContext()
          : base("MultasDbConnectionString", throwIfV1Schema: false) {
      }

      static ApplicationDbContext() {
         // Set the database intializer which is run once during application start
         // This seeds the database with admin user credentials and admin role
         Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
      }

      public static ApplicationDbContext Create() {
         return new ApplicationDbContext();
      }

      // dados específicos sobre as Multas
      // definir as 'tabelas' da minha base de dados
      public virtual DbSet<Viaturas> Viaturas { get; set; }
      public virtual DbSet<Condutores> Condutores { get; set; }
      public virtual DbSet<Agentes> Agentes { get; set; }
      public virtual DbSet<Multas> Multas { get; set; }


      protected override void OnModelCreating(DbModelBuilder modelBuilder) {
         modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
         modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
         base.OnModelCreating(modelBuilder);
      }

   }
}