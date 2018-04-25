using AzR.Utilities.Attributes;

namespace AzR.Core.Config
{
    [IgnoreEntity]
    public interface IEntity<T> : IBaseEntity
    {
        T Id { get; set; }
        string LoginId { get; set; }
    }

}
