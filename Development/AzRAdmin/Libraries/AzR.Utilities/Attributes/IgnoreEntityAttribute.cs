using System;

namespace AzR.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IgnoreEntityAttribute : Attribute
    {
    }
}