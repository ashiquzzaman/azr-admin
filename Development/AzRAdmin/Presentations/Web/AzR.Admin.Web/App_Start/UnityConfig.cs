using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using AzR.Core.Business;
using AzR.Core.Config;
using AzR.Core.Repositories;
using Unity.Mvc5;
using VelocityWorkFlow.Web.Controllers.Mvc;

namespace VelocityWorkFlow.Web
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

            container.RegisterType<IOrganizationRepository, OrganizationRepository>();
            container.RegisterType<ILoginHistoryRepository, LoginHistoryRepository>();
            container.RegisterType<IRoleRepository, RoleRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            #endregion

            #region SERVICE

            container.RegisterType<IBaseService, BaseService>();
            #endregion


            container.RegisterType<UserAuthController>(new InjectionConstructor(typeof(IBaseService)));
            container.RegisterType<UserProfileController>(new InjectionConstructor(typeof(IBaseService)));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

        }
    }
}