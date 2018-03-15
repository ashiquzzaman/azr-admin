using AzR.Core.Config;
using AzR.Core.Entities;
using AzR.Core.Enumerations;
using AzR.Core.Repositoies.Interface;
using AzR.Core.ViewModels.Admin;
using System;
using System.Data.Entity;
using System.Linq.Expressions;

namespace AzR.Core.Repositoies.Implementation
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(DbContext context) : base(context)
        {
        }

        public MenuViewModel EntityFactory(Menu model)
        {
            return new MenuViewModel
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                Url = model.Url,
                ParentId = model.ParentId,
                MenuOrder = model.MenuOrder,
                Icon = model.Icon,
                MenuType = model.ParentId == null ? MenuType.Module : MenuType.Menu,
                RoleId = model.RoleId,
                IsActive = model.IsActive

            };
        }
        public Menu ModelFactory(MenuViewModel model)
        {
            return new Menu
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                Url = model.Url,
                ParentId = model.ParentId,
                MenuOrder = model.MenuOrder,
                Icon = model.Icon,
                MenuType = model.ParentId == null ? MenuType.Module : MenuType.Menu,
                RoleId = model.RoleId,
                IsActive = model.IsActive

            };
        }
        public Expression<Func<Menu, MenuViewModel>> ModelExpression
        {
            get
            {
                return model => new MenuViewModel
                {
                    Id = model.Id,
                    DisplayName = model.DisplayName,
                    Url = model.Url,
                    ParentId = model.ParentId,
                    MenuOrder = model.MenuOrder,
                    Icon = model.Icon,
                    MenuType = model.ParentId == null ? MenuType.Module : MenuType.Menu,
                    RoleId = model.RoleId,
                    IsActive = model.IsActive
                };
            }
        }

    }
}