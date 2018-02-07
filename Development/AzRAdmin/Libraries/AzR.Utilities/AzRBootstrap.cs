using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AzR.Utilities
{
    public class AzRBootstrap
    {
        public static CompositionContainer Container { get; private set; }
        public static bool IsIntialized { get; private set; }
        public static List<Assembly> Assemblies { get; private set; }
        public static void Intialize(List<string> pluginFolders)
        {
            if (IsIntialized) return;

            var catalog = new AggregateCatalog();

            foreach (var plugin in pluginFolders)
            {
                var files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", plugin), "AzR.Plugin.*.dll", SearchOption.AllDirectories).ToList();
                foreach (var file in files)
                {
                    catalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFrom(file)));
                }
            }
            Container = new CompositionContainer(catalog);

            Container.ComposeParts();
            IsIntialized = true;
        }

        public static T GetInstance<T>(string contractName = null)
        {
            var type = default(T);
            if (Container == null) return type;

            try
            {
                if (!string.IsNullOrWhiteSpace(contractName))
                    type = Container.GetExportedValue<T>(contractName);
                else
                    type = Container.GetExportedValue<T>();
            }
            catch
            {

            }

            return type;
        }

        public static void Intialize()
        {

            try
            {
                if (!IsIntialized)
                {
                    Assemblies = new List<Assembly>();
                    var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "AzR.*.dll", SearchOption.AllDirectories)
                        .Where(o => !o.Replace(AppDomain.CurrentDomain.BaseDirectory, "").Contains(@"obj\")).ToList();
                    var catalog = new AggregateCatalog();

                    foreach (var file in files)
                    {
                        var assembly = Assembly.LoadFrom(file);
                        Assemblies.Add(assembly);
                        catalog.Catalogs.Add(new AssemblyCatalog(assembly));
                    }

                    Container = new CompositionContainer(catalog);

                    Container.ComposeParts(Container);
                    IsIntialized = true;

                }
            }
            catch (Exception)
            {
                IsIntialized = false;
                throw;
            }



        }

        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var currentAssemblies = Assemblies;

            return currentAssemblies.FirstOrDefault(assembly =>
                assembly.FullName == args.Name || assembly.GetName().Name == args.Name);
        }
    }
}