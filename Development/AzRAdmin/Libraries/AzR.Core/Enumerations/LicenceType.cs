using System.ComponentModel.DataAnnotations;

namespace AzR.Core.Enumerations
{
    public enum LicenceType
    {
        [Display(Name = "Owner")]
        Owner = 1,
        [Display(Name = "All Module")]
        AllModule = 2,
        [Display(Name = "Without Advance Inventory")]
        WithoutAdvanceInventory = 3,
        [Display(Name = "Without Sales")]
        WithoutBooking = 4,
        [Display(Name = "Basic Feature")]
        BasicFeature = 5
    }
}
