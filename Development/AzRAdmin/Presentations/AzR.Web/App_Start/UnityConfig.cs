using AzR.Core.AppContexts;
using AzR.Core.Repositoies.Implementation;
using AzR.Core.Repositoies.Interface;
using AzR.Core.Services.Implementation;
using AzR.Core.Services.Interface;
using AzR.Web.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;

namespace AzR.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();


            #region CONFIG

            //DB Connection
            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            //Identity Entity Config
            container.RegisterType(typeof(UserManager<>), new InjectionConstructor(typeof(IUserStore<>)));
            container.RegisterType<IUser>(new InjectionFactory(c => c.Resolve<IUser>()));
            container.RegisterType(typeof(IUserStore<>), typeof(UserStore<>));
            //container.RegisterType<IdentityUser, ApplicationUser>(new ContainerControlledLifetimeManager());
            //container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IIdentityMessageService, EmailService>();

            #endregion

            #region REPOSITORY
            container.RegisterType<ILoginHistoryRepository, LoginHistoryRepository>();
            container.RegisterType<IOrganizationRepository, OrganizationRepository>();
            container.RegisterType<IMenuRepository, MenuRepository>();
            container.RegisterType<IRoleRepository, RoleRepository>();
            container.RegisterType<IUserPrivilegeRepository, UserPrivilegeRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            #endregion

            #region SERVICE
            container.RegisterType<IBaseManager, BaseManager>();
            container.RegisterType<IModuleManager, ModuleManager>();
            container.RegisterType<IOrganizationManager, OrganizationManager>();
            container.RegisterType<IMenuManager, MenuManager>();
            container.RegisterType<IRoleManager, RoleManager>();
            container.RegisterType<IUserPrivilegeManager, UserPrivilegeManager>();
            container.RegisterType<IUserManager, UserManager>();

            #endregion


            container.RegisterType<UserAuthController>(new InjectionConstructor(typeof(IBaseManager)));
            container.RegisterType<UserProfileController>(new InjectionConstructor(typeof(IBaseManager)));

            container.RegisterType<UserAuthApiController>(new InjectionConstructor(typeof(IBaseManager)));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

        }
    }
}