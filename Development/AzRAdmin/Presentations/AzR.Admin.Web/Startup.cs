using Microsoft.Owin;
using Owin;
using AzR.Admin.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace AzR.Admin.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
