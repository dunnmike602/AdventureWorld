using System.Reflection;

namespace ReflectionHelper.Core
{
    public class MemberDescriptor : DescriptorBase
    {
        private TypeDescriptor _containingType;
        public MemberInfo CurrentMember { get; private set; }

        public TypeDescriptor ContainingType
        {
            get => _containingType;
            set
            {
                _containingType = value;
                OnPropertyChanged();
            }
        }

        private MemberDescriptor()
        {

        }

        public MemberDescriptor(MemberInfo member, TypeDescriptor containingType)
        {
            ContainingType = containingType;
            CurrentMember = member;
        }

        public MemberDescriptor Clone(TypeDescriptor parentType)
        {
            return new MemberDescriptor()
            {
                CurrentMember = CurrentMember,
                ContainingType = parentType,
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