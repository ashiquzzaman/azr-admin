using AzR.Core.AppContexts;
using AzR.Core.Repositoies.Implementation;
using AzR.Core.Services.Interface;
using AzR.Web.Controllers;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;
using Unity.RegistrationByConvention;

namespace AzR.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();



            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());

            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

            container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(WebApiApplication).Assembly),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(UserRepository).Assembly),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            //container.RegisterType<DbContext, StudentDbContext>(typeof(StudentDbContext).Name, new HierarchicalLifetimeManager());
            //var repos = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"), "AzR.*.Core.dll", SearchOption.AllDirectories).ToList();

            //foreach (var file in repos)
            //{
            //    container.RegisterTypes(
            //        AllClasses.FromAssemblies(Assembly.LoadFrom(file))
            //        .Where(t => t.Name.EndsWith("Repository") || t.Name.EndsWith("Service")),
            //        WithMappings.FromMatchingInterface,
            //        getInjectionMembers: new InjectionParameter(typeof(IBaseService))
            //        );

            //}



            //container.RegisterTypes(
            //    AllClasses.FromLoadedAssemblies(),  //uses reflection
            //    WithMappings.FromMatchingInterface, //Matches Interfaces to implementations by name
            //    WithName.Default);

            //container.RegisterTypes(
            //    AllClasses.FromAssemblies(typeof(UserRepository).Assembly)
            //        .Where(t => t.Name.EndsWith("Repository")),
            //    WithMappings.FromAllInterfaces,
            //    WithName.TypeName,
            //    WithLifetime.Transient);

            //container.RegisterTypes(
            //    AllClasses.FromAssemblies(typeof(BaseService).Assembly)
            //        .Where(t => t.Name.EndsWith("Service")),
            //    WithMappings.FromAllInterfaces);







            container.RegisterType<UserAuthController>(new InjectionConstructor(typeof(IBaseService)));
            container.RegisterType<UserProfileController>(new InjectionConstructor(typeof(IBaseService)));

            container.RegisterType<UserAuthApiController>(new InjectionConstructor(typeof(IBaseService)));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

        }
    }
}