using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectionHelper.Core.Extensions
{
    public static class TypeTranslator
    {
        private static readonly Dictionary<Type, string> DefaultDictionary = new Dictionary<Type, string>
        {
            {typeof(int), "int"},
            {typeof(uint), "uint"},
            {typeof(long), "long"},
            {typeof(ulong), "ulong"},
            {typeof(short), "short"},
            {typeof(ushort), "ushort"},
            {typeof(byte), "byte"},
            {typeof(sbyte), "sbyte"},
            {typeof(bool), "bool"},
            {typeof(float), "float"},
            {typeof(double), "double"},
            {typeof(decimal), "decimal"},
            {typeof(char), "char"},
            {typeof(string), "string"},
            {typeof(object), "object"},
            {typeof(void), "void"}
        };

        public static string GetFriendlyName(this Type type, Dictionary<Type, string> translations, bool useFullTypeName)
        {
            if (translations.ContainsKey(type))
            {
                return translations[type];
            }
            else if (type.IsArray)
            {
                return GetFriendlyName(type.GetElementType(), translations, useFullTypeName) + "[]";
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GetGenericArguments()[0].GetFriendlyName(useFullTypeName) + "?";
            }
            else if (type.IsGenericType)
            {
               return type.Name.Split('`')[0] + "<" + string.Join(", ",
                           type.GetGenericArguments().Select(genricType => genricType.GetFriendlyName(useFullTypeName)).ToArray()) + ">";
            }
            else
            {
                var varianceFlag = string.Empty;

                if (type.IsGenericParameter)
                {
                    var attributes = type.GenericParameterAttributes;

                    if (GenericParameterAttributes.None != (attributes & GenericParameterAttributes.Covariant))
                    {
                        varianceFlag = "out ";
                    }

                    if (GenericParameterAttributes.None != (attributes & GenericParameterAttributes.Contravariant))
                    {
                        varianceFlag = "in ";
                    }
                }

                var typeName = useFullTypeName ? type.FullName ?? type.Name : type.Name;

                return $"{varianceFlag}{typeName}";
            }
        }

        public static string GetFriendlyName(this Type type, bool useFullTypeName)
        {
            return type.GetFriendlyName(DefaultDictionary, useFullTypeName);
        }
    }
}