using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReflectionHelper.Attributes;
using ReflectionHelper.Core.Extensions;

namespace ReflectionHelper.Core
{
    public class TypeDescriptor : BuildableDescriptorBase
    {
        public Type CurrentType { get; private set; }

        public NamespaceDescriptor ContainingNamespace { get; private set; }
        
        public IList<MemberDescriptor> MemberDescriptors { get; set; } = new List<MemberDescriptor>();

        private TypeDescriptor()
        {

        }

        public TypeDescriptor(Type type, NamespaceDescriptor containingNamespace)
        {
            CurrentType = type;
            ContainingNamespace = containingNamespace;

            Build();
        }

        protected sealed override void Build()
        {
            var displayableMembers = CurrentType.GetMembers().Where(member => (member is MethodInfo info && !info.GetIsSpecialButNotConstructor() && !CurrentType.IsEnum) || 
                ((CurrentType.IsEnum && member.GetType().Name == "MdFieldInfo") || member.MemberType == MemberTypes.Constructor || member.MemberType == MemberTypes.Property
                 || member.MemberType == MemberTypes.Event) && !member.HasAttribute(typeof(IgnoreInObjectBrowser)));
   
            foreach (var member in displayableMembers.OrderBy(member => member.MemberType).ThenBy(member => member.Name))
            {
                MemberDescriptors.Add(new MemberDescriptor(member, this) { DisplayName = member.GetMemberDefinitionWithoutModifiers(false), Image = GetImage(member),
                    ImageDescription = member.GetMemberFullDefinition()});
            }
        }

        private string GetImage(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Constructor || member.MemberType == MemberTypes.Method)
            {
                return "/Resources/Images/method.gif";
            }

            if (member.MemberType == MemberTypes.Field && CurrentType.IsEnum)
            {
                return "/Resources/Images/enumitem.png";
            }

            if (member.MemberType == MemberTypes.Property)
            {
                return "/Resources/Images/property.png";
            }

            if (member.MemberType == MemberTypes.Event)
            {
                return "/Resources/Images/event.png";
            }

            return string.Empty;
        }

        public bool ContainsAnySearchedItems()
        {
            return IsSearchSuccessful || MemberDescriptors.Any(md => md.IsSearchSuccessful);
        }

        public TypeDescriptor Clone(NamespaceDescriptor parentNameSpace)
        {
            return new TypeDescriptor
            {
                CurrentType = CurrentType,
                ContainingNamespace = parentNameSpace,
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
