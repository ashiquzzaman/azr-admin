using System.Data.Entity.Migrations;
using AzR.Core.AppContexts;

namespace AzR.Core.Migrations
{


    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var init = new InitializeConfig { Context = context };
            init.InitializeOrganization();
            init.InitializeRole();
            init.InitializeAdmin();
        }
    }
}
