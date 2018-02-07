using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace AzR.Core.Web.MEF
{
    public class AzRViewEngine : RazorViewEngine
    {
        private List<string> _plugins = new List<string>();
        public AzRViewEngine()
        {
            var plugins = !Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"))
                ? Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins")).ToList()
                : Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Areas")).ToList();

            plugins.ForEach(s =>
            {
                var di = new DirectoryInfo(s);
                _plugins.Add(di.Name);
            });

            ViewLocationFormats = GetViewLocations(base.ViewLocationFormats);
            MasterLocationFormats = GetMasterLocations(base.MasterLocationFormats);
            PartialViewLocationFormats = PartialViewLocations(base.PartialViewLocationFormats);

        }
        public AzRViewEngine(List<string> pluginFolders)
        {
            _plugins = pluginFolders;

            ViewLocationFormats = GetViewLocations(base.ViewLocationFormats);
            MasterLocationFormats = GetMasterLocations(base.MasterLocationFormats);
            PartialViewLocationFormats = PartialViewLocations(base.PartialViewLocationFormats);
        }

        public string[] GetViewLocations(string[] basePath)
        {
            var baseViews = basePath.ToList();
            var views = new List<string> {
                "~/bin/Views/{1}/{0}.cshtml",
                "~/Areas/Views/{1}/{0}.cshtml" };

            _plugins.ForEach(plugin =>
                views.Add("~/Plugins/" + plugin + "/Views/{1}/{0}.cshtml")
            );
            _plugins.ForEach(plugin =>
                views.Add("~/bin/" + plugin + "/Views/{1}/{0}.cshtml")
            );
            _plugins.ForEach(plugin =>
                views.Add("~/Areas/" + plugin + "/Views/{1}/{0}.cshtml")
            );
            views.AddRange(baseViews);
            return views.ToArray();
        }

        public string[] GetMasterLocations(string[] basePath)
        {
            var baseViews = basePath.ToList();

            var masterPages = new List<string> {
                "~/bin/Views/Shared/{0}.cshtml",
                "~/Areas/Views/Shared/{0}.cshtml" };


            _plugins.ForEach(plugin =>
                masterPages.Add("~/Plugins/" + plugin + "/Views/Shared/{0}.cshtml")
            );
            _plugins.ForEach(plugin =>
                masterPages.Add("~/bin/" + plugin + "/Views/Shared/{0}.cshtml")
            );
            _plugins.ForEach(plugin =>
                masterPages.Add("~/Areas/" + plugin + "/Views/Shared/{0}.cshtml")
            );
            masterPages.AddRange(baseViews);
            return masterPages.ToArray();
        }

        public string[] PartialViewLocations(string[] basePath)
        {
            var baseViews = basePath.ToList();
            var views = new List<string> {
                "~/bin/Views/{1}/{0}.cshtml",
                "~/Areas/Views/{1}/{0}.cshtml" };

            _plugins.ForEach(plugin =>
                views.Add("~/Plugins/" + plugin + "/Views/{1}/{0}.cshtml")
            );
            _plugins.ForEach(plugin =>
                views.Add("~/bin/" + plugin + "/Views/{1}/{0}.cshtml")
            );
            _plugins.ForEach(plugin =>
                views.Add("~/Areas/" + plugin + "/Views/{1}/{0}.cshtml")
            );
            views.AddRange(baseViews);
            return views.ToArray();
        }

    }
}