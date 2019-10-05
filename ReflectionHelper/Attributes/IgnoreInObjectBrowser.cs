using System;

namespace ReflectionHelper.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class IgnoreInObjectBrowser : Attribute
    {
    }
}
