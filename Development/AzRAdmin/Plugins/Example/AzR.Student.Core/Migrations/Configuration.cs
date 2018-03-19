using AzR.Core.Config;
using System.Data.Entity.SqlServer;

namespace AzR.Student.Core.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SqlClient", new SqlServerMigrationSqlGenerator());
            SetHistoryContextFactory("System.Data.SqlClient", (conn, schema) => new CustomHistoryContext(conn, schema));

        }

        protected override void Seed(StudentDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
