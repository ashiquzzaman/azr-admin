using AzR.Core.Entities;
using AzR.Core.IdentityConfig;
using AzR.Core.Migrations;
using AzR.Utilities.Attributes;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;

namespace AzR.Core.Config
{
    public class CheckAndMigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration>
        : IDatabaseInitializer<TContext>
        where TContext : DbContext
        where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        public virtual void InitializeDatabase(TContext context)
        {
            //var config = new DbMigrationsConfiguration<ApplicationDbContext>
            //{
            //    AutomaticMigrationsEnabled = true

            //};

            //var migrator = new DbMigrator(config);
            //migrator.Update();

            // var configuration = new Configuration();
            //var migrator = new DbMigrator(configuration);

            //var scriptor = new MigratorScriptingDecorator(migrator);
            //var script = scriptor.ScriptUpdate(sourceMigration: null, targetMigration: null);
            //migrator.Update();

            //var pending = migrator.GetPendingMigrations();


            var migratorBase = (MigratorBase)new DbMigrator(Activator.CreateInstance<TMigrationsConfiguration>());
            if (migratorBase.GetPendingMigrations().Any())
            {
                migratorBase.Update();
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,
        int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(
                new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            Database.Initialize(false);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //Database.SetInitializer<ApplicationDbContext>(null);

            //modelBuilder.HasDefaultSchema("C##AZRADMIN");

            base.OnModelCreating(modelBuilder);

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

            //FOR FLUENT API CONFIG
            var addMethod = typeof(ConfigurationRegistrar)
                .GetMethods()
                .Single(m =>
                    m.Name == "Add"
                    && m.GetGenericArguments().Any(a => a.Name == "TEntityType"));

            foreach (var assembly in AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.GetName().Name != "EntityFramework"))
            {
                var configTypes = assembly
                    .GetTypes()
                    .Where(t => t.BaseType != null
                                && t.BaseType.IsGenericType
                                && t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

                foreach (var type in configTypes)
                {
                    var entityType = type.BaseType.GetGenericArguments().Single();
                    var entityConfig = assembly.CreateInstance(type.FullName);
                    addMethod.MakeGenericMethod(entityType)
                        .Invoke(modelBuilder.Configurations, new object[] { entityConfig });
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