using AzR.Core.Config;
using AzR.Core.Entities;
using AzR.Core.ViewModels.Admin;
using System;
using System.Linq.Expressions;

namespace AzR.Core.Repositoies.Interface
{
    public interface IMenuRepository : IRepository<Menu>
    {
        MenuViewModel EntityFactory(Menu model);
        Menu ModelFactory(MenuViewModel model);
        Expression<Func<Menu, MenuViewModel>> ModelExpression { get; }
    }
}