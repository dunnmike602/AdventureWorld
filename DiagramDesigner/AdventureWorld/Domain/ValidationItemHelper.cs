using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using DiagramDesigner.AdventureWorld.Domain.Interfaces;
using DiagramDesigner.Extensions;

namespace DiagramDesigner.AdventureWorld.Domain
{
    public static class ValidationItemHelper
    {
        private static IEnumerable<PropertyInfo> GetPropertiesForCriteria(object source, Type validationItemType, 
            ValidationTypes validationType)
        {
            var sourceObjectType = source.GetType();

            return sourceObjectType.GetProperties().
                Where(m => Attribute.IsDefined(m, validationItemType) &&
                    ((m.GetCustomAttribute(typeof(NotNullAttribute)) != null
                        && (m.GetValue(source) == null || string.IsNullOrWhiteSpace(m.GetValue(source).ToString())))
                  || (m.GetCustomAttribute(typeof(NonEmptyCollectionAttribute)) != null &&
                    m.GetFirstListValue(source) == null )
                     || (m.GetCustomAttribute(typeof(NoneZeroAttribute)) != null &&
                         Convert.ToInt32(m.GetValue(source)) == 0 )
                     )
                   && (validationType == ValidationTypes.All || m.GetValidationItemAttribute().ValidationType
                   == validationType)
                   && (!(source is IValidationFilter validationFilterObject) || validationFilterObject.ShouldValidate(m))
                   );
        }

        public static void Add(ObservableCollection<ValidationItemAttribute> collection, ValidationItemAttribute[]
            itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                var item1 = item;

                if (!collection.Where((m => m.ControlId == item1.ControlId && m.ValidationType ==
                                           item1.ValidationType)).Any())
                {

                    collection.Add(item);
                }
            }
        }

        public static void Remove(ObservableCollection<ValidationItemAttribute> collection, ValidationTypes 
            validationType, Guid? controlId = null)
        {
            var item = collection.FirstOrDefault(m => m.ValidationType == validationType &&
                m.ControlId == controlId);

            if(item != null)
            {
                collection.Remove(item);
            }
        }

        public static ValidationItemAttribute[] CreateOptionsValidationItems<TSourceType>(TSourceType source, 
            Func<Guid?, bool > action, ValidationCategories catagory, string name, ValidationTypes validationType = ValidationTypes.All,
            Guid? controlId = null)
        {
            var items = new List<ValidationItemAttribute>();

            var propertiesToProcess = GetPropertiesForCriteria(source, typeof(ValidationItemAttribute), validationType);

            foreach (var validationItemAttribute in
                propertiesToProcess.Select(nextProperty => nextProperty.GetValidationItemAttribute()))
            {
                validationItemAttribute.Action = action;
                validationItemAttribute.ValidationCategory = catagory;
                validationItemAttribute.ControlId = controlId;
                validationItemAttribute.Name = name;

                items.Add(validationItemAttribute);
            }

            return items.ToArray();
        }

        public static ChangedEventArgs CreateEventArguments(object source, string propertyName,
            Type attributeType)
        {
            var sourceObjectType = source.GetType();
            var propertyInfo = sourceObjectType.GetProperty(propertyName);
            var validationItem = (ValidationItemAttribute) propertyInfo.GetCustomAttribute(typeof(
                ValidationItemAttribute));

            return new ChangedEventArgs
                {
                    ValidationType = validationItem.ValidationType,
                };
       }
    }
}