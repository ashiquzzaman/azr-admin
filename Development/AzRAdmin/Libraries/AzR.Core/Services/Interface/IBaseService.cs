﻿using AzR.Core.Entities;
using AzR.Core.HelperModels;
using System.Threading.Tasks;

namespace AzR.Core.Services.Interface
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
        Task<bool> IsActive(string userName);
    }
}
