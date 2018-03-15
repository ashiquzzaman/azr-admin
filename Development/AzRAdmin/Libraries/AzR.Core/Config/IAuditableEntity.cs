namespace AzR.Core.Config
{
    public interface IAuditableEntity
    {
        int CreatedBranch { get; set; }
        long CreatedBy { get; set; }
        long CreatedTime { get; set; }
        long ModifiedBy { get; set; }
        long ModifiedTime { get; set; }
        bool IsActive { get; set; }
    }
}
