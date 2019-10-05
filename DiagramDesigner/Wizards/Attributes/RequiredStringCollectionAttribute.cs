using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DiagramDesigner.Wizards.Attributes
{
    public class RequiredStringCollectionAttribute : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = value is IEnumerable<string> targetObject && !string.IsNullOrWhiteSpace(targetObject.FirstOrDefault());

            if (isValid)
            {
                return ValidationResult.Success;
            }

            var memberNames = validationContext.MemberName != null ? new string[] { validationContext.MemberName } : null;
            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName), memberNames);
           
        }
    }
}