using System.Data.Entity.Migrations;
using AzR.Core.Config;

namespace AzR.Core.Migrations
{


    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var init = new InitializeConfig { Context = context };
            init.InitializeRole();
            init.InitializeOrganization();
            init.InitializeAdmin();
        }
    }
}
