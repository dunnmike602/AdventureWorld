using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectionHelper.Core
{
    public class AssemblyDescriptor : BuildableDescriptorBase
    {
        public Assembly CurrentAssembly { get; }

        public string Location { get; set; }

        public IList<NamespaceDescriptor> NamespaceDescriptors { get; set; } = new List<NamespaceDescriptor>();

        public AssemblyDescriptor(Assembly assembly)
        {
            CurrentAssembly = assembly;

            Build();
        }

        protected sealed override void Build()
        {
            var nameSpaces = CurrentAssembly.GetTypes().Where(ApplicationDomainDescriptor.NamespaceSelectionExpression)
                .Select(type => type.Namespace).Where(ns => !string.IsNullOrWhiteSpace(ns)).Distinct();

            foreach (var nameSpace in nameSpaces.OrderBy(ns => ns))
            {
                NamespaceDescriptors.Add(new NamespaceDescriptor(this, nameSpace){Image = "/Resources/Images/namespace.png", ImageDescription = $"namespace {nameSpace}"});
            }
        }
    }
}
