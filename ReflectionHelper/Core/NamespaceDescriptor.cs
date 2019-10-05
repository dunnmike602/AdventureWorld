using System;
using System.Collections.Generic;
using System.Linq;
using PhilosophicalMonkey;
using ReflectionHelper.Core.Extensions;

namespace ReflectionHelper.Core
{
    public class NamespaceDescriptor : BuildableDescriptorBase
    {
        public AssemblyDescriptor ContainingAssembly { get; private set; }

        public IList<TypeDescriptor> TypeDescriptors { get; set; } = new List<TypeDescriptor>();

        private NamespaceDescriptor()
        {

        }

        public NamespaceDescriptor(AssemblyDescriptor containingAssembly, string ns)
        {
            ContainingAssembly = containingAssembly;
            DisplayName = ns;

            Build();
        }

        protected sealed override void Build()
        {
            foreach (var type in Reflect.OnTypes.GetTypesFromNamespace(ContainingAssembly.CurrentAssembly, DisplayName).Where(ApplicationDomainDescriptor.
                TypeSelectionExpression).OrderBy(type => type.Name))
            {
               TypeDescriptors.Add(new TypeDescriptor(type, this)
                {
                    DisplayName = type.GetFriendlyName(false),
                    Image = GetImageFromType(type),
                    ImageDescription = type.GetTypeDefinition(),
                    TypeDefinition = type.GetTypeDefinition()
                });

                ApplicationDomainDescriptor.CurrentTypeCount++;

                ApplicationDomainDescriptor.ProgressMonitor?.Report((int)(ApplicationDomainDescriptor.CurrentTypeCount / ApplicationDomainDescriptor.TypeCount * 100.0M));
            }
        }

        private static string GetImageFromType(Type type)
        {
            if (type.IsClass && type.IsSubclassOf(typeof(MulticastDelegate)))
            {
                return "/Resources/Images/delegate.png";
            }

            if (type.IsEnum)
            {
                return "/Resources/Images/enum.png";
            }

            if (type.IsValueType)
            {
                return "/Resources/Images/struct.png";
            }

            if (type.IsClass)
            {
                return "/Resources/Images/class.png";
            }

            if (type.IsInterface)
            {
                return "/Resources/Images/interface.png";
            }
            
            return string.Empty;
        }

        public bool ContainsAnySearchedItems()
        {
            return IsSearchSuccessful || TypeDescriptors.Any(td => td.ContainsAnySearchedItems());
        }

        public NamespaceDescriptor Clone()
        {
            return new NamespaceDescriptor
            {
                ContainingAssembly = ContainingAssembly,
                DisplayName = DisplayName,
                IsSearchSuccessful = IsSearchSuccessful,
                CodeComments = CodeComments,
                Image = Image,
                IsSelected = false,
                TypeDefinition = TypeDefinition,
                ImageDescription = ImageDescription
            };
        }
    }
}
