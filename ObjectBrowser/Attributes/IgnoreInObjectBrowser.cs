using System;

namespace MLDComputing.ObjectBrowser.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class IgnoreInObjectBrowser : Attribute
    {
    }
}
