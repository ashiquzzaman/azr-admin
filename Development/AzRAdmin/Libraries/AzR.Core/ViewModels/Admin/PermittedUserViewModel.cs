namespace AzR.Core.ViewModels.Admin
{
    public class PermittedUserViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool IsPermitted { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int ReviewLevel { get; set; }
        public string ModuleName { get; set; }

    }

}
