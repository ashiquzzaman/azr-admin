using AzR.Core.AuditLogs;
using AzR.Core.Entities;
using AzR.Core.IdentityConfig;
using AzR.Core.Notifications;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace AzR.Core.Config
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,
        int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {

        }
        #region General

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<UserMenu> UserMenus { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }

        #endregion

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

            //  Database.SetInitializer(new AppDatabaseInitializer());

            //modelBuilder.HasDefaultSchema("C##AZRADMIN");
            //modelBuilder
            //    .Properties()
            //    .Where(p => p.PropertyType == typeof(string) &&
            //                !p.Name.Contains("Id") &&
            //                !p.Name.Contains("Provider"))
            //    .Configure(p => p.HasMaxLength(256));




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