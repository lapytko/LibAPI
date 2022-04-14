using System;

namespace LibAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class IgnorePropertyAttribute : Attribute
    {
    }
}