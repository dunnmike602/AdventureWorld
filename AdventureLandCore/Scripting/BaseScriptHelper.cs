using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using AdventureLandCore.Services.CoreApi;

namespace AdventureLandCore.Scripting
{
    public static class BaseScriptHelper
    {
        public static IList<Assembly> GetStandardReference()
        {
            var mscorlib = typeof(object).Assembly;
            var xml = typeof(XmlDocument).Assembly;
            var systemCore = typeof(System.Linq.Enumerable).Assembly;
            var adApi = typeof(AdventureApi).Assembly;

            return new[] {mscorlib, systemCore, xml, adApi };
        }
    }
}