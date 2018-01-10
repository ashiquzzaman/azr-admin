using System.Collections.Generic;
using System.Web.Mvc;

namespace AzR.Core.MvcAuth
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
    }
}