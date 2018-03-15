using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;

namespace AzR.Core.Config
{
    public class CustomHistoryContext : HistoryContext
    {
        public CustomHistoryContext(
          DbConnection existingConnection,
          string defaultSchema)
            : base(existingConnection, defaultSchema)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(255).IsRequired();
        }
    }
}
