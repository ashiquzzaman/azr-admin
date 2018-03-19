using AzR.Core.AuditLogs;
using AzR.Core.Entities;
using AzR.Utilities.Securities;
using System.Threading.Tasks;

namespace AzR.Core.Services.Interface
{
    public interface IBaseService
    {
        void LogOutTime(int userId);
        LoginHistory LoginTime(string userName);
        string LoginId(int userId);
        AppUserPrincipal AppUser(LoginHistory login);
        Branch GetOwner();
        void SetCookie(string userName);
        Task<bool> IsActive(string userName);
    }
}
