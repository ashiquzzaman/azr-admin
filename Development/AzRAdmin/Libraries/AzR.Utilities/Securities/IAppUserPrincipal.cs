using System.Security.Principal;

namespace AzR.Utilities.Securities
{
    public interface IAppUserPrincipal : IPrincipal
    {
        string Id { get; set; }
        int UserId { get; set; }
        int ProfileId { get; set; }
        string UserName { get; set; }
        string UniqueName { get; set; }
        string Name { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        long Expaired { get; set; }
        int ActiveBranchId { get; set; }
        int ParentBranchId { get; set; }
        int ActiveRoleId { get; set; }
        string RoleIds { get; set; }
        string ActiveRoleName { get; set; }
        string RoleNames { get; set; }
        bool HasAllPermission { get; }
        string PermittedBranchs { get; set; }
    }
}