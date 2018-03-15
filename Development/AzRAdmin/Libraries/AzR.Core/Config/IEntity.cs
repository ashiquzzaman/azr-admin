namespace AzR.Core.Config
{
    public interface IEntity<T>
    {
        T Id { get; set; }
        string LoginId { get; set; }
    }

}
