using AzR.Core.AuditLogs;
using AzR.Core.Entities;
using AzR.Core.IdentityConfig;
using AzR.Core.Notifications;
using AzR.Utilities.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

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

        public override int SaveChanges()
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var changes = 0;
                    var addedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

                    if (addedEntries.Count > 0)
                    {
                        base.SaveChanges();
                        foreach (var entry in addedEntries)
                        {
                            var audit = WriteLog.Create(entry, 1);
                            if (audit == null) continue;
                            AuditLogs.Add(audit);
                            var notify = Notification.ActionNotifyForGroup(entry, 1, audit.Id);
                            Notifications.Add(notify);
                        }

                        changes = base.SaveChanges();
                    }
                    var deleteEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
                    if (deleteEntries.Count > 0)
                    {
                        foreach (var entry in deleteEntries)
                        {
                            var audit = WriteLog.Create(entry, 3);
                            if (audit == null) continue;
                            AuditLogs.Add(audit);
                            var notify = Notification.ActionNotifyForGroup(entry, 3, audit.Id);
                            Notifications.Add(notify);

                        }
                        changes = base.SaveChanges();
                    }
                    var modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();

                    if (modifiedEntries.Count > 0)
                    {
                        foreach (var entry in modifiedEntries)
                        {
                            var audit = WriteLog.Create(entry, 2);
                            if (audit == null) continue;
                            AuditLogs.Add(audit);
                            var notify = Notification.ActionNotifyForGroup(entry, 2, audit.Id);
                            Notifications.Add(notify);
                        }
                        changes = base.SaveChanges();
                    }

                    NotificationHub.Notify();
                    scope.Complete();
                    return changes;
                }

            }
            catch (DbEntityValidationException ex)
            {
                var outputLines = new List<string>();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format(
                            "- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                GeneralHelper.WriteValue(string.Join("\n", outputLines));
                throw new Exception(string.Join(",", outputLines.ToArray()));

            }
        }

        public override Task<int> SaveChangesAsync()
        {

            try
            {
                using (var scope = new TransactionScope())
                {
                    var changes = Task.FromResult(0);
                    var addedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

                    if (addedEntries.Count > 0)
                    {
                        base.SaveChanges();
                        foreach (var entry in addedEntries)
                        {
                            var audit = WriteLog.Create(entry, 1);
                            if (audit == null) continue;
                            AuditLogs.Add(audit);
                            var notify = Notification.ActionNotifyForGroup(entry, 1, audit.Id);
                            Notifications.Add(notify);
                        }

                        changes = base.SaveChangesAsync();
                    }

                    var deleteEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
                    if (deleteEntries.Count > 0)
                    {
                        foreach (var entry in deleteEntries)
                        {
                            var audit = WriteLog.Create(entry, 3);
                            if (audit == null) continue;
                            AuditLogs.Add(audit);
                            var notify = Notification.ActionNotifyForGroup(entry, 3, audit.Id);
                            Notifications.Add(notify);

                        }
                        changes = base.SaveChangesAsync();
                    }
                    var modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();

                    if (modifiedEntries.Count > 0)
                    {
                        foreach (var entry in modifiedEntries)
                        {
                            var audit = WriteLog.Create(entry, 2);
                            if (audit == null) continue;
                            AuditLogs.Add(audit);
                            var notify = Notification.ActionNotifyForGroup(entry, 2, audit.Id);
                            Notifications.Add(notify);
                        }
                        changes = base.SaveChangesAsync();
                    }

                    NotificationHub.Notify();
                    scope.Complete();
                    return changes;
                }

            }
            catch (DbEntityValidationException ex)
            {
                var outputLines = new List<string>();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format(
                            "- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                GeneralHelper.WriteValue(string.Join("\n", outputLines));
                throw new Exception(string.Join(",", outputLines.ToArray()));

            }

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