using System.Threading.Tasks;
using AzR.Core.Entities;
using AzR.Core.HelperModels;

namespace AzR.Core.Services.Interface
{
    public interface IBaseManager
    {
        void LoginTime(int shopId, int userId);
        void LogOutTime(int userId);
        void LoginTime(string userName);
        string LoginId(int userId);
        CmsUserViewModel CmsUser(string userName);
        Organization GetOwner();
        void SetCookie(string userName);
        Task<bool> IsActive(string userName);
    }
}
