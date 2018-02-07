using System;
using System.Web.Http;
using System.Web.Mvc;
using AzR.Admin.Web.Areas.HelpPage.ModelDescriptions;
using AzR.Admin.Web.Areas.HelpPage.Models;

namespace AzR.Admin.Web.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        //public HelpController()
        //    : this(GlobalConfiguration.Configuration)
        //{
        //}
        public HelpController()

        {
            Configuration = GlobalConfiguration.Configuration;
        }

        //public HelpController(HttpConfiguration config)
        //{
        //    Configuration = config;
        //}

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            return PartialView(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return PartialView(apiModel);
                }
            }

            return PartialView(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return PartialView(modelDescription);
                }
            }

            return PartialView(ErrorViewName);
        }
    }
}