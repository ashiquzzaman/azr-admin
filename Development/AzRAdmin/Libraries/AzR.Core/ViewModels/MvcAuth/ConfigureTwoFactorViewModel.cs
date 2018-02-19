using System.Collections.Generic;
using AzR.Core.HelperModels;

namespace AzR.Core.ViewModels.MvcAuth
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<DropDownItem> Providers { get; set; }
    }
}