using System.ComponentModel.DataAnnotations;

namespace AzR.Core.Enumerations
{
    public enum MenuLocation
    {
        [Display(Name = "Left Menu")]

        LeftMenu = 1,
        [Display(Name = "Right Setting")]
        RightSetting = 2,
    }
}
