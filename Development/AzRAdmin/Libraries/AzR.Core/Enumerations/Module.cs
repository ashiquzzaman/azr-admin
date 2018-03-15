using System.ComponentModel.DataAnnotations;

namespace AzR.Core.Enumerations
{
    public enum Module
    {
        [Display(Name = "General")]
        General = 1,
        [Display(Name = "Task Management")]
        Tasks = 2,
        [Display(Name = "Booking")]
        Booking = 3,
        [Display(Name = "Inventory")]
        Inventory = 4,
        [Display(Name = "Purchase & Sales")]
        PurchaseSales = 5,
        [Display(Name = "Order Management")]
        Order = 6,
        [Display(Name = "Accounting")]
        Accounting = 7,
        [Display(Name = "Party")]
        Client = 8,
        [Display(Name = "Human Resource")]
        HR = 9,
        [Display(Name = "Payroll")]
        Payroll = 10,
        [Display(Name = "Leave Management")]
        Leave = 11,
        [Display(Name = "Opening Balance")]
        OpeningBalance = 12,
        [Display(Name = "Closing Balance")]
        ClosingBalance = 13,
        [Display(Name = "Site")]
        Site = 14,
        [Display(Name = "MENU")]
        Menu = 15

    }
}
