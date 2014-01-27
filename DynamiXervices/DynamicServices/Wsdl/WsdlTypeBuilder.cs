using System;
using System.Collections.Generic;
using System.Linq;

namespace Taga.DynamicServices.Wsdl
{
    public static class WsdlTypeBuilder
    {
        private static readonly IDictionary<string, Type> DynamicTypes = new Dictionary<string, Type>();

        public static void BuildTypes(SimpleWsdl wsdl)
        {
            lock (DynamicTypes)
            {
                var wsdlTypes = wsdl.Types.Types;
                DeclareTypes(wsdlTypes);
                BuildTypes(wsdlTypes);
                SetServiceTypes(wsdl.Services);
            }
        }

        private static void DeclareTypes(IEnumerable<WsdlType> wsdlTypes)
        {
            foreach (var wsdlType in wsdlTypes)
                DynamicTypes.Add(wsdlType.FullTypeName, wsdlType.DeclaredType);
        }

        private static void BuildTypes(IEnumerable<WsdlType> wsdlTypes)
        {
            foreach (var wsdlType in wsdlTypes)
            {
                SetPropertyTypes(wsdlType);
                BuildType(wsdlType);
            }
        }

        private static void SetPropertyTypes(WsdlType wsdlType)
        {
            foreach (var pocoProp in wsdlType.Poco.Properties)
                pocoProp.Type = GetType(wsdlType.Properties.First(p => p.Name == pocoProp.Name).FullTypeName);
        }

        private static void BuildType(WsdlType wsdlType)
        {
            DynamicTypes[wsdlType.FullTypeName] = wsdlType.BuildType();
        }

        private static void SetServiceTypes(WsdlServices wsdlServices)
        {
            foreach (var method in wsdlServices.Services.SelectMany(wsdlService => wsdlService.Methods))
            {
                method.Type = GetType(method.FullTypeName);
                foreach (var input in method.Inputs)
                    input.Type = GetType(input.FullTypeName);
            }
        }

        private static Type GetType(string fullTypeName)
        {
            var isArray = fullTypeName.EndsWith("[]");

            if (isArray)
                return GetType(fullTypeName.Substring(0, fullTypeName.Length - 2)).MakeArrayType();

            return DynamicTypes.ContainsKey(fullTypeName)
                       ? DynamicTypes[fullTypeName]
                       : Type.GetType(fullTypeName);
        }
    }
}
