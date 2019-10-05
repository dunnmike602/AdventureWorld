using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml;

namespace ReflectionHelper.Core.Extensions
{
    public static class Extensions
    {
        private static XmlDocument _docuDoc;
        private static string _oldDocumentName;

        public static string GetSummaryCommentForType(this string assemblyName, Type type, string xmlDocumentationFilesLocation)
        {
            if (string.IsNullOrWhiteSpace(xmlDocumentationFilesLocation))
            {
                return string.Empty;
            }

            var docuPath = Path.ChangeExtension(Path.Combine(xmlDocumentationFilesLocation, assemblyName), ".xml");

            if (_oldDocumentName != docuPath)
            {
                _docuDoc = null;
            }

            if (_docuDoc != null || File.Exists(docuPath))
            {
                _oldDocumentName = docuPath;

                if (_docuDoc == null)
                {
                    _docuDoc = new XmlDocument();
                    _docuDoc.Load(docuPath);
                }

                var path = "T:" + type.FullName;

                var xmlClass = _docuDoc.SelectSingleNode("//member[starts-with(@name, '" + path + "')]");

                return xmlClass?.InnerText.TrimSpaceAndNewLines() ?? string.Empty;
            }

            return string.Empty;
        }

        public static (string Summary, Dictionary<string, string> ParameterComments, string ReturnValue) GetMethodDocumentation(this string assemblyName, MemberInfo memberInfo, string xmlDocumentationFilesLocation)
        {
            var parameterDocumentation = new Dictionary<string,string>();

            if (string.IsNullOrWhiteSpace(xmlDocumentationFilesLocation))
            {
                return (string.Empty, parameterDocumentation, string.Empty);
            }

            var docuPath = Path.ChangeExtension(Path.Combine(xmlDocumentationFilesLocation, assemblyName), ".xml");

            if (_oldDocumentName != docuPath)
            {
                _docuDoc = null;
            }

            if (_docuDoc != null || File.Exists(docuPath))
            {
                _oldDocumentName = docuPath;

                if (_docuDoc == null)
                {
                    _docuDoc = new XmlDocument();
                    _docuDoc.Load(docuPath);
                }

                var locator = GetSelector(memberInfo) + ":";

                var path = locator + memberInfo.DeclaringType?.FullName + "." + memberInfo.Name;

                var methodDocumentation = _docuDoc.SelectSingleNode("//member[starts-with(@name, '" + path + "')]");

                var summaryText = methodDocumentation?.SelectSingleNode("summary")?.InnerText.TrimSpaceAndNewLines() ?? string.Empty;

                var parameterNodes = methodDocumentation?.SelectNodes("param");

                if (parameterNodes != null)
                {
                    foreach (XmlNode parameterNode in parameterNodes)
                    {
                        parameterDocumentation.Add(parameterNode.Attributes["name"].Value, parameterNode.InnerText.TrimSpaceAndNewLines());
                    }
                }

                var returnsText = methodDocumentation?.SelectSingleNode("returns")?.InnerText.TrimSpaceAndNewLines() ?? string.Empty;

                return (summaryText, parameterDocumentation, returnsText);
            }

            return (string.Empty, parameterDocumentation, string.Empty);
        }

        private static string GetSelector(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Method)
            {
                return "M";
            }

            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return "F";
            }

            if (memberInfo.MemberType == MemberTypes.Property)
            {
                return "P";
            }

            return string.Empty;
        }

        public static string TrimSpaceAndNewLines(this string source)
        {
            return source.Trim().Replace(Environment.NewLine, string.Empty).Replace("\t", string.Empty).ReplaceMultipleSpaces();
        }

        public static string ReplaceMultipleSpaces(this string source)
        {
            return Regex.Replace(source, @"\s+", " ");
        }

        public static string GetModifier(this MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Constructor)
            {
                var constructorInfo = memberInfo as ConstructorInfo;

                return constructorInfo.IsPublic ? "public" : "private";
            }
            else if (memberInfo.MemberType == MemberTypes.Method)
            {
                var methodInfo = memberInfo as MethodInfo;

                return methodInfo.IsPublic ? "public" : "private";
            }
            else if (memberInfo.MemberType == MemberTypes.Event)
            {
               return "public";
            }
            else if (memberInfo.MemberType == MemberTypes.Property)
            {
                var propertyInfo = memberInfo as PropertyInfo;

                return propertyInfo?.GetSetMethod() != null || propertyInfo?.GetGetMethod() != null ? "public" : "private";
            }
            else if (memberInfo.MemberType == MemberTypes.Field)
            {
                return string.Empty;
            }

            return "NOT SUPPORTED";
        }

        public static string GetReturnType(this MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Method)
            {
                var methodInfo = memberInfo as MethodInfo;

                return methodInfo?.ReturnType.GetReturnType(false);
            }

            if (memberInfo.MemberType == MemberTypes.Property)
            {
                var propertyInfo = memberInfo as PropertyInfo;

                return propertyInfo?.PropertyType.GetReturnType(true);
            }

            if (memberInfo.MemberType == MemberTypes.Constructor)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        public static string GetModifier(this Type source)
        {
            return source.IsPublic ? "public" : "private";
        }

        public static string GetSealed(this Type source)
        {
            return source.IsSealed && !source.GetIsDelegate() && !source.IsEnum ? "sealed" : string.Empty;
        }

        public static bool GetIsDelegate(this Type source)
        {
            return source.IsSubclassOf(typeof(Delegate));
        }

        public static string GetAbstract(this Type source)
        {
            return source.IsAbstract && !source.IsInterface ? "abstract" : string.Empty;
        }

        public static string GetReturnType(this Type sourceType, bool useFullName)
        {
            var returnType = sourceType.GetIsDelegate() ? sourceType.GetMethod("Invoke")?.ReturnType : sourceType;

            if (returnType == null || returnType == typeof(void))
            {
                return "void";
            }

            return returnType.GetFriendlyName(useFullName);
        }

        public static bool HasAttribute(this Type type, Type attribute)
        {
            return type.GetCustomAttributes(false).Any(attr => attr.GetType().Name.IsEqualTo(attribute.Name));
        }

        public static bool HasAttribute(this Assembly assembly, Type attribute)
        {
            return assembly.GetCustomAttributes(false).Any(attr => attr.GetType().Name.IsEqualTo(attribute.Name));
        }

        public static bool HasAttribute(this MemberInfo memberInfo, Type attribute)
        {
            return memberInfo.GetCustomAttributes(false).Any(attr => attr.GetType().Name.IsEqualTo(attribute.Name));
        }
        
        public static string GetTypeClass(this Type source)
        {
            if (source.GetIsDelegate())
            {
               return "delegate";
            }

            if (source.IsInterface)
            {
                return "interface";
            }

            if (source.IsClass)
            {
                return "class";
            }

            if (source.IsValueType)
            {
                return "struct";
            }

            return source.IsEnum ? "enum" : string.Empty;
        }

        public static string GetInheritedClass(this Type source)
        {
            return source.BaseType != null && source.BaseType != typeof(object) && !source.GetIsDelegate() && !source.IsEnum ?
                $": {source.BaseType.GetFriendlyName(false)}" : string.Empty;
        }

        public static string GetArgumentsFromType(this Type[] types)
        {
            if (types.Length == 0)
            {
                return string.Empty;
            }

            var parameterList = string.Empty;

            var count = 1;

            foreach (var parameter in types)
            {
                parameterList += parameter.Name + $" arg{count}, ";
                count++;
            }

            return "(" + parameterList.Trim().Trim(',') + ")";
        }

        public static string GetArgumentsFromMethod(this ParameterInfo[] parameterInfos)
        {
            if (parameterInfos.Length == 0)
            {
                return "()";
            }

            var parameterList = parameterInfos.Aggregate(string.Empty,
                    (current, parameter) => current + $" {parameter.ParameterType.GetFriendlyName(true)},");
           
            return "(" + parameterList.Trim().Trim(',') + ")";
        }

        public static string GetDelegateParameters(this Type source)
        {
            return source.GetIsDelegate() ? source.GetGenericArguments().GetArgumentsFromType() : string.Empty;
        }

        public static bool GetIsSpecialButNotConstructor(this MethodInfo methodInfo)
        {
            return methodInfo.IsSpecialName && !methodInfo.IsConstructor;
        }

        public static string GetMethodName(this MethodInfo methodInfo)
        {
            if (methodInfo.IsGenericMethod)
            {
                var genericArguments = string.Empty;

                foreach (var parameter in methodInfo.GetGenericArguments())
                {
                    genericArguments += $" {parameter.Name}, ";
                }

                return $"{methodInfo.Name}<{genericArguments.Trim().Trim(',')}>";
            }

            return methodInfo.Name;
        }

        public static string GetMemberDefinitionWithoutModifiers(this MemberInfo memberInfo, bool hideGettersAndSetters)
        {
           if (memberInfo.MemberType == MemberTypes.Constructor)
            {
                var constructorInfo = memberInfo as ConstructorInfo;

                return $"{memberInfo.DeclaringType?.Name}{constructorInfo?.GetParameters().GetArgumentsFromMethod()}".ReplaceMultipleSpaces();
            }
            else if (memberInfo.MemberType == MemberTypes.Method)
            {
                var methodInfo = memberInfo as MethodInfo;

                return $"{methodInfo.GetMethodName()}{methodInfo?.GetParameters().GetArgumentsFromMethod()}".ReplaceMultipleSpaces();
            }
            else if (memberInfo.MemberType == MemberTypes.Event)
            {
                var eventInfo = memberInfo as EventInfo;

                return eventInfo?.Name;
            }
            else if (memberInfo.MemberType == MemberTypes.Field)
            {
                var propertyInfo = memberInfo as FieldInfo;

                return propertyInfo?.Name;
            }
            else if (memberInfo.MemberType == MemberTypes.Property)
            {
                var propertyInfo = memberInfo as PropertyInfo;

                var body = string.Empty;

                if (hideGettersAndSetters && propertyInfo != null)
                {
                    if (propertyInfo.CanRead && propertyInfo.CanWrite)
                    {
                        body = " { get; set; }";
                    }
                    else if (propertyInfo.CanRead)
                    {
                        body = " { get; }";
                    }
                    else
                    {
                        body = " { set; }";
                    }
                }

                return $"{memberInfo.Name} {body}".ReplaceMultipleSpaces();
            }

            return "NOT SUPPORTED";
        }

        public static string GetMemberFullDefinition(this MemberInfo member)
        {
            var eventName = string.Empty;

            if (member is EventInfo eventInfo)
            {
                eventName = $"event {eventInfo.EventHandlerType.GetFriendlyName(true)}";
            }
           
            return $"{member.GetModifier()} {member.GetReturnType()} {eventName} {member.GetMemberDefinitionWithoutModifiers(true)}".ReplaceMultipleSpaces();
        }

        public static string GetTypeDefinition(this Type source)
        {
            return ($"{source.GetModifier()} {source.GetSealed()} {source.GetAbstract()} {source.GetTypeClass()} " +
                   $"{source.GetReturnType()} {source.GetFriendlyName(false)} {source.GetDelegateParameters()} {source.GetInheritedClass()}").ReplaceMultipleSpaces();
        }

        public static string GetNameOnly(this Assembly source)
        {
            return source.GetName().Name;
        }

        public static bool IsEqualTo(this string source, string argument)
        {
            return string.Compare(source, argument, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool IsEqualToAny(this string source, params string[] arguments)
        {
            return arguments.Any(argument => string.Compare(source, argument, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public static string GetNameAndVersion(this Assembly assembly)
        {
            return assembly.GetNameOnly() + " " + assembly.GetVersion();
        }

        public static string GetVersion(this Assembly assembly)
        {
            var ver = assembly.GetName().Version;

            return $"[{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}]";
        }

        public static IList<Assembly> GetAssembliesByName(this AppDomain appDomain, params string[] assemblyNames)
        {
            return appDomain.GetAssemblies().Where(assembly =>
                !assemblyNames.Any() || assembly.GetNameOnly().IsEqualToAny(assemblyNames)).OrderBy(sortingAssembly =>
                Array.IndexOf(assemblyNames, sortingAssembly.GetNameOnly())).ToList();
        }

        public static bool IsAnonymousType(this Type type)
        {
            var hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any();
            var nameContainsAnonymousType = type.FullName != null && type.FullName.Contains("AnonymousType");
            return hasCompilerGeneratedAttribute && nameContainsAnonymousType && string.IsNullOrWhiteSpace(type.Namespace);
        }
    }
}
