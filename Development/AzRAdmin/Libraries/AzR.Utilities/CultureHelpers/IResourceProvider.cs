namespace AzR.Utilities.CultureHelpers
{
    public interface IResourceProvider
    {
        object GetResource(string name, string culture);
    }
}
