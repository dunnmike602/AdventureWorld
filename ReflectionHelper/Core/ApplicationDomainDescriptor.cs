using System;
using System.Collections.Generic;
using System.Linq;
using ReflectionHelper.Attributes;
using ReflectionHelper.Core.Extensions;

namespace ReflectionHelper.Core
{
    public class ApplicationDomainDescriptor : BuildableDescriptorBase
    {
        public static Func<Type, bool> NamespaceSelectionExpression { get; } = (type) =>
            (!Filter.ContainsKey(type.Assembly.GetNameOnly()) ||
             (type.Namespace.IsEqualToAny(Filter[type.Assembly.GetNameOnly()].ToArray()) ||
              !Filter[type.Assembly.GetNameOnly()].Any()));

        public static Func<Type, bool> TypeSelectionExpression { get; } = (type) =>
            !type.IsAnonymousType() && type.IsPublic && !type.HasAttribute(typeof(IgnoreInObjectBrowser));

        public static string XmlDocumentationFilesLocation { get; set; }

        public static IProgress<int> ProgressMonitor { get; set; }

        internal static decimal TypeCount { get; set; }

        internal static decimal CurrentTypeCount { get; set; }
        
        public AppDomain CurrentDomain { get; }

        public static Dictionary<string, List<string>> Filter { get; private set; }

        public IList<AssemblyDescriptor> AssemblyDescriptors { get; set; } = new List<AssemblyDescriptor>();

        public ApplicationDomainDescriptor(AppDomain appDomain, Dictionary<string, List<string>> filter = null)
        {
            CurrentDomain = appDomain;
            Filter = filter ?? new Dictionary<string, List<string>>();

            foreach (var key in Filter.Keys.ToList())
            {
                if (filter != null && filter[key] == null)
                {
                    filter[key] = new List<string>();
                }
            }

            Build();
        }
        
        protected sealed override void Build()
        {
            var assembliesToProcess = CurrentDomain.GetAssembliesByName(Filter.Keys.ToArray()).Where(assembly => !assembly.IsDynamic && 
                !assembly.HasAttribute(typeof(IgnoreInObjectBrowser))).ToList();

            TypeCount = assembliesToProcess.SelectMany(assembly => assembly.GetTypes().Where(TypeSelectionExpression)).Where(NamespaceSelectionExpression).Count();

            CurrentTypeCount = 0;

            foreach (var assembly in assembliesToProcess)
            {
                AssemblyDescriptors.Add(new AssemblyDescriptor(assembly)
                {
                    Location = assembly.Location,
                    DisplayName = assembly.GetNameAndVersion(),
                    Image = "/Resources/Images/assembly.png",
                    ImageDescription = assembly.GetNameAndVersion(),
                });
            }
        }

        public List<NamespaceDescriptor> Search(string searchText)
        {
            // Mark all items that have the text in them
            var allNameSpaces = AssemblyDescriptors.SelectMany(ad => ad.NamespaceDescriptors).Cast<DescriptorBase>().ToList();
            var allTypes = AssemblyDescriptors.SelectMany(ad => ad.NamespaceDescriptors).SelectMany(ns => ns.TypeDescriptors).Cast<DescriptorBase>().ToList();
            var allMembers = AssemblyDescriptors.SelectMany(ad => ad.NamespaceDescriptors)
                .SelectMany(ns => ns.TypeDescriptors).SelectMany(td => td.MemberDescriptors).Cast<DescriptorBase>().ToList();

            var allDescriptors = allMembers.Union(allTypes).Union(allNameSpaces).ToList();

            foreach (var descriptor in allDescriptors)
            {
                descriptor.IsSearchSuccessful = descriptor.DisplayName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            // Create new structure to hold the filtered list
            var filteredNamespaces = new List<NamespaceDescriptor>();

            // Find all namespaces that have the text somewhere in them
            foreach (var namespaceDescriptor in AssemblyDescriptors.SelectMany(ad => ad.NamespaceDescriptors))
            {
                if(namespaceDescriptor.ContainsAnySearchedItems())
                {
                    var newNamespace = namespaceDescriptor.Clone();

                    filteredNamespaces.Add(newNamespace);

                    // Find all the types that have text in them
                    foreach (var typeDescriptor in namespaceDescriptor.TypeDescriptors)
                    {
                        var newTypeDescriptor = typeDescriptor.Clone(newNamespace);

                        if (typeDescriptor.ContainsAnySearchedItems())
                        {
                            newNamespace.TypeDescriptors.Add(newTypeDescriptor);
                        }

                        // Find all the members that have text in them
                        foreach (var memberDescriptor in typeDescriptor.MemberDescriptors)
                        {
                            if (memberDescriptor.IsSearchSuccessful)
                            {
                                var newMemberDescriptor = memberDescriptor.Clone(newTypeDescriptor);
                                newTypeDescriptor.MemberDescriptors.Add(newMemberDescriptor);
                            }
                        }
                    }
                }
            }
            
            return filteredNamespaces;
        }
    }
}