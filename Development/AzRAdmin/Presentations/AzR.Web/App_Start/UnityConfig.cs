using AzR.Core.Config;
using AzR.Core.IdentityConfig;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.RegistrationByConvention;

namespace AzR.Web
{
    public static class UnityConfig
    {


        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            IdentityContainer(container);

            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies(),  //uses reflection
                WithMappings.FromMatchingInterface, //Matches Interfaces to implementations by name
                WithName.Default);

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

        }

        public static void IdentityContainer(UnityContainer container)
        {
            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());

            container.RegisterType<IUserStore<ApplicationUser, int>, ApplicationUserStore>();
            container.RegisterType<IRoleStore<ApplicationRole, int>, ApplicationRoleStore>();

            container.RegisterType<IUser>(new InjectionFactory(c => c.Resolve<IUser>()));
            container.RegisterType<UserManager<ApplicationUser, int>>(new HierarchicalLifetimeManager());

            container.RegisterType<ApplicationUserManager>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationRoleManager>(new HierarchicalLifetimeManager());

            container.RegisterType<IIdentityMessageService, EmailService>();

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<IOwinContext>(new InjectionFactory(o => HttpContext.Current.GetOwinContext()));
            container.RegisterType<ApplicationSignInManager>();

            container.RegisterType(typeof(ISecureDataFormat<>), typeof(SecureDataFormat<>));
            container.RegisterType<ISecureDataFormat<AuthenticationTicket>, SecureDataFormat<AuthenticationTicket>>();
            container.RegisterType<ISecureDataFormat<AuthenticationTicket>, TicketDataFormat>();
            container.RegisterType<IDataSerializer<AuthenticationTicket>, TicketSerializer>();
            container.RegisterType<IDataProtector>(
                new InjectionFactory(c => new DpapiDataProtectionProvider().Create("ASP.NET Identity")));


            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

            container.RegisterType<IRoleRepository, RoleRepository>(new HierarchicalLifetimeManager());

            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());

        }

    }
}