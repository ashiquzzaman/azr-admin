using System;
using System.Linq.Expressions;
using AzR.Core.AppContexts;
using AzR.Core.Entities;
using AzR.Core.ViewModels.Admin;

namespace AzR.Core.Repositoies.Interface
{
    public interface IMenuRepository : IRepository<Menu>
    {
        MenuViewModel EntityFactory(Menu model);
        Menu ModelFactory(MenuViewModel model);
        Expression<Func<Menu, MenuViewModel>> ModelExpression { get; }
    }
}