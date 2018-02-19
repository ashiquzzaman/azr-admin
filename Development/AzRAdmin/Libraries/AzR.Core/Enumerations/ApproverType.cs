using System.ComponentModel.DataAnnotations;

namespace AzR.Core.Enumerations
{
    public enum ApproverType
    {
        [Display(Name = "Initiator")]
        Initiator = 1,
        [Display(Name = "n-th Approver")]
        Approver = 2,
        [Display(Name = "Final Approver")]
        FinalApprover = 3,
        [Display(Name = "Executor")]
        Executor = 0
    }
}
