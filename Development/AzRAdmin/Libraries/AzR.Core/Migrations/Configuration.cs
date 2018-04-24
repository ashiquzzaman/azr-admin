using AzR.Core.Config;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.SqlServer;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace AzR.Core.Migrations
{


    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private readonly bool _pendingMigrations;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SqlClient", new SqlServerMigrationSqlGenerator());
            SetHistoryContextFactory("System.Data.SqlClient", (conn, schema) => new CustomHistoryContext(conn, schema));

        }

        protected override void Seed(ApplicationDbContext context)
        {
            var init = new InitializeConfig { Context = context };
            init.InitializeBranch();
            init.InitializeRole();
            init.InitializeAdmin();
        }
    }

    internal class AppDatabaseInitializer : CreateAndMigrateDatabaseInitializer<ApplicationDbContext, Configuration>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var init = new InitializeConfig { Context = context };
            init.InitializeBranch();
            init.InitializeRole();
            init.InitializeAdmin();
        }
    }

    internal class CreateAndMigrateDatabaseInitializer<TContext, TConfiguration> : CreateDatabaseIfNotExists<TContext>, IDatabaseInitializer<TContext>
         where TContext : DbContext
         where TConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        private readonly DbMigrationsConfiguration _configuration;
        public CreateAndMigrateDatabaseInitializer()
        {
            _configuration = new TConfiguration();
        }
        public CreateAndMigrateDatabaseInitializer(string connection)
        {
            Contract.Requires(!string.IsNullOrEmpty(connection), "connection");

            _configuration = new TConfiguration
            {
                TargetDatabase = new DbConnectionInfo(connection)
            };
        }
        void IDatabaseInitializer<TContext>.InitializeDatabase(TContext context)
        {
            var doseed = !context.Database.Exists();
            // && new DatabaseTableChecker().AnyModelTableExists(context);
            // check to see if to seed - we 'lack' the 'AnyModelTableExists' - could be copied/done otherwise if needed...

            var migrator = new DbMigrator(_configuration);

            var _pendingMigrations = migrator.GetPendingMigrations().Any();
            var _historyRepository = migrator.GetType().GetField("_historyRepository", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(migrator);
            var _existingConnection = _historyRepository.GetType().BaseType.GetField("_existingConnection", BindingFlags.Instance | BindingFlags.NonPublic);
            _existingConnection.SetValue(_historyRepository, null);


            // if (doseed || !context.Database.CompatibleWithModel(throwIfNoMetadata: false))
            if (_pendingMigrations)
                migrator.Update();

            // move on with the 'CreateDatabaseIfNotExists' for the 'Seed'
            base.InitializeDatabase(context);
            if (doseed)
            {
                Seed(context);
                context.SaveChanges();
            }
        }
        protected override void Seed(TContext context)
        {
        }
    }

}
