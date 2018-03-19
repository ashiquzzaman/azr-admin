using AzR.Core.AuditLogs;
using AzR.Core.Notifications;
using System;
using System.Data.Entity;

namespace AzR.Student.Core
{
    public class StudentDbContext : DbContext
    {
        private readonly bool _isMigrationMode;

        public StudentDbContext() : base("DefaultConnection")//base(AppIdentity.AppUser.ConnectionName)
        {
        }
        public DbSet<Models.Student> Students { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            base.OnModelCreating(modelBuilder);

            //  Database.SetInitializer(new AppDatabaseInitializer());

            //modelBuilder.HasDefaultSchema("C##AZRADMIN");
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Ignore<AuditLog>();

            //modelBuilder.Ignore<Notification>();

            //modelBuilder.Ignore<UserNotification>();

            Database.SetInitializer<StudentDbContext>(null);

        }

    }
}
