using AzR.Core.Config;
using AzR.Core.IdentityConfig;
using AzR.Core.Repositoies.Implementation;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Registration;
using Unity.RegistrationByConvention;

namespace AzR.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            IdentityContainer(container);

            var repos = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"), "AzR.*.Core.dll", SearchOption.AllDirectories).ToList();

            foreach (var file in repos)
            {
                var assembly = Assembly.LoadFrom(file);
                var drivenType = assembly.GetTypes().FirstOrDefault(t => typeof(DbContext).IsAssignableFrom(t));
                if (drivenType == null) continue;

                container.RegisterType(typeof(DbContext), drivenType, drivenType.Name, new HierarchicalLifetimeManager(), new InjectionMember[] { });

                container.RegisterTypes(
                    AllClasses.FromAssemblies(assembly)
                    .Where(t => t.Name.EndsWith("Repository")),
                    WithMappings.FromMatchingInterface,
                    getInjectionMembers: type => new List<InjectionMember> { new InjectionConstructor(container.Resolve<DbContext>(drivenType.Name)) }
                    // getInjectionMembers: type => new List<InjectionMember> { new InjectionConstructor(new ResolvedParameter<DbContext>(drivenType.Name)) }
                    );

                container.RegisterTypes(
                    AllClasses.FromAssemblies(assembly)
                        .Where(t => t.Name.EndsWith("Service")),
                    WithMappings.FromMatchingInterface,
                    WithName.Default
                 );
            }

            container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(WebApiApplication).Assembly),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

        }

        public static void IdentityContainer(UnityContainer container)
        {
            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());

            container.RegisterType<IUserStore<ApplicationUser, int>, ApplicationUserStore>();
            container.RegisterType<IRoleStore<ApplicationRole, int>, ApplicationRoleStore>();

            //container.RegisterType<IUserStore<ApplicationUser, int>, ApplicationUserStore>(new InjectionConstructor(new ResolvedParameter<ApplicationDbContext>(typeof(DbContext).Name)));
            //container.RegisterType<IRoleStore<ApplicationRole, int>, ApplicationRoleStore>(new InjectionConstructor(new ResolvedParameter<ApplicationDbContext>(typeof(DbContext).Name)));

            container.RegisterType<IUser>(new InjectionFactory(c => c.Resolve<IUser>()));
            container.RegisterType<UserManager<ApplicationUser, int>>(new HierarchicalLifetimeManager());

            container.RegisterType<ApplicationUserManager>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationRoleManager>(new HierarchicalLifetimeManager());

            container.RegisterType<IIdentityMessageService, EmailService>();

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<ApplicationSignInManager>();

            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

            container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(UserRepository).Assembly),
                WithMappings.FromMatchingInterface,
                WithName.Default);
        }

    }
}