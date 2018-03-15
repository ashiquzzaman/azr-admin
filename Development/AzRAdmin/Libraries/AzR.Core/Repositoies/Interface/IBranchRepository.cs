using AzR.Core.Config;
using AzR.Core.Entities;
using AzR.Core.ViewModels.Admin;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace AzR.Core.Repositoies.Interface
{
    public interface IBranchRepository : IRepository<Branch>
    {
        IQueryable<Branch> GetAllActive();
        IQueryable<Branch> GetAllDeactive();
        Branch GetBy(int id);
        IQueryable<Branch> TakeBy(int number);

        BranchViewModel EntityFactory(Branch model);
        Branch ModelFactory(BranchViewModel model);
        Expression<Func<Branch, BranchViewModel>> ModelExpression { get; }
    }
}
