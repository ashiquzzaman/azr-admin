using AzR.Core.Entity;
using AzR.Core.ModelConfig;

namespace AzR.Core.Business
{
    public interface IBaseService
    {
        void LoginTime(int shopId, int userId);
        void LogOutTime(int userId);
        void LoginTime(string userName);
        string LoginId(int userId);
        CmsUserViewModel CmsUser(string userName);
        Organization GetOwner();
        void SetCookie(string userName);
        bool IsActive(string userName);
    }
}
