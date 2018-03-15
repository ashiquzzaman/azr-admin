using System.ComponentModel.DataAnnotations;

namespace AzR.Core.Enumerations
{
    public enum MenuType
    {
        [Display(Name = "Module")]
        Module = 0,
        [Display(Name = "Menu")]
        Menu = 1,
        [Display(Name = "Tab")]
        Tab = 2,

    }
}