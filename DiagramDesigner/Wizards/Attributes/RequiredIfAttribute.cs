using System.ComponentModel.DataAnnotations;

namespace DiagramDesigner.Wizards.Attributes
{
    public class RequiredIfAttribute : RequiredAttribute
    {
        private readonly string _dependentProperty;

        public RequiredIfAttribute( string dependentProperty)
            : base()
        {
            this._dependentProperty = dependentProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(this._dependentProperty);
            var dependentvalue = (bool)field.GetValue(validationContext.ObjectInstance);

            return dependentvalue ? base.IsValid(value, validationContext) : ValidationResult.Success;
        }
    }
}
