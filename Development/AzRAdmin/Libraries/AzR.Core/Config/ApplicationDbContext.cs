using AzR.Core.Entities;
using AzR.Core.IdentityConfig;
using AzR.Utilities.Attributes;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;

namespace AzR.Core.Config
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,
        int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            base.OnModelCreating(modelBuilder);
            Database.SetInitializer<ApplicationDbContext>(null);

            //  Database.SetInitializer(new AppDatabaseInitializer());

            //modelBuilder.HasDefaultSchema("C##AZRADMIN");
            //modelBuilder
            //    .Properties()
            //    .Where(p => p.PropertyType == typeof(string) &&
            //                !p.Name.Contains("Id") &&
            //                !p.Name.Contains("Provider"))
            //    .Configure(p => p.HasMaxLength(256));


            var assList = AppDomain.CurrentDomain.GetAssemblies()
                .Where(s => s.FullName.Contains("AzR"))
                .Where(s => s.FullName.Contains("Core"));

            var builder = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var assembly in assList)
            {
                var entityTypes = assembly
                    .GetTypes()
                    .Where(t => typeof(IBaseEntity).IsAssignableFrom(t)
                                && !t.IsDefined(typeof(IgnoreEntityAttribute), false));

                foreach (var type in entityTypes)
                {
                    builder.MakeGenericMethod(type)
                        .Invoke(modelBuilder, new object[] { });
                }


            }

            modelBuilder.Entity<Branch>()
                .HasOptional(p => p.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(p => p.ParentId);

            modelBuilder.Entity<Menu>()
                .HasOptional(p => p.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(p => p.ParentId);

            modelBuilder.Entity<ApplicationRole>()
                .ToTable("Roles")
                .Property(c => c.Name).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users")
                .Property(c => c.UserName).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users")
                .Property(c => c.Email).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<ApplicationUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("UserRoles");

            modelBuilder.Entity<ApplicationUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("UserLogins");

            modelBuilder.Entity<ApplicationUserClaim>()
                .ToTable("UserClaims")
                .Property(u => u.ClaimType).HasMaxLength(150);

            modelBuilder.Entity<ApplicationUserClaim>()
                .ToTable("UserClaims")
                .Property(u => u.ClaimValue).HasMaxLength(500);

            //modelBuilder.Conventions.Add(new DateConvention());

        }

    }
}