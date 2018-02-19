using System;
using System.Linq;
using System.Linq.Expressions;
using AzR.Core.AppContexts;
using AzR.Core.Entities;
using AzR.Core.ViewModels.Admin;

namespace AzR.Core.Repositoies.Interface
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        IQueryable<Organization> GetAllActive();
        IQueryable<Organization> GetAllDeactive();
        Organization GetBy(int id);
        IQueryable<Organization> TakeBy(int number);

        OrganizationViewModel EntityFactory(Organization model);
        Organization ModelFactory(OrganizationViewModel model);
        Expression<Func<Organization, OrganizationViewModel>> ModelExpression { get; }
    }
}
