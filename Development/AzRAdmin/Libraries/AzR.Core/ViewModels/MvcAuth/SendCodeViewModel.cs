using System.Collections.Generic;
using AzR.Core.HelperModels;

namespace AzR.Core.ViewModels.MvcAuth
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<DropDownItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}