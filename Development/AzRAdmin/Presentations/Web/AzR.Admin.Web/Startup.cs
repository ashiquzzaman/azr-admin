using Microsoft.Owin;
using Owin;
using VelocityWorkFlow.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace VelocityWorkFlow.Web
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
