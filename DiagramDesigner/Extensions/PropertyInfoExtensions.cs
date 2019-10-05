using System;
using System.Collections.ObjectModel;
using System.Reflection;
using DiagramDesigner.AdventureWorld.Domain;

namespace DiagramDesigner.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static int GetIntegerValue(this PropertyInfo propertyInfo, object targetObject)
        {
            return (int)propertyInfo.GetValue(targetObject, null);
        }

        public static object GetValue(this PropertyInfo propertyInfo, object targetObject)
        {
            return propertyInfo.GetValue(targetObject, null);
        }

        public static string GetFirstListValue(this PropertyInfo propertyInfo, object targetObject)
        {
            var listValue = (ObservableCollection<string>)propertyInfo.GetValue(targetObject, null);

            return listValue.Count == 0 || string.IsNullOrWhiteSpace(listValue[0])
                ? null : listValue[0];
        }
        
        public static ValidationItemAttribute GetValidationItemAttribute(this PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttribute(typeof(ValidationItemAttribute));

            return (ValidationItemAttribute) attributes;
        }

        public static object GetCustomAttribute(this PropertyInfo propertyInfo, Type attributeType)
        {
            var attributes = propertyInfo.GetCustomAttributes(attributeType, true);

            if(attributes.Length == 0)
            {
                return null;
            }

            return attributes[0];
        }
    }
}