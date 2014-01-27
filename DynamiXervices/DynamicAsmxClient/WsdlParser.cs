using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Services.Description;
using System.Xml;

namespace Taga.DynamicServices.AsmxClient
{
    internal class WsdlParser
    {
        private readonly string _wsdlUri;

        private Assembly _webServiceAssembly;

        internal IEnumerable<Type> AvailableTypes { get { return _webServiceAssembly.GetExportedTypes(); } }
        
        internal WsdlParser(string wsdlUri)
        {
            _wsdlUri = wsdlUri;
        }

        internal void Parse()
        {
            _webServiceAssembly = BuildAssemblyFromWsdl(_wsdlUri);
        }

        private static Assembly BuildAssemblyFromWsdl(string wsdlUri)
        {
            if (String.IsNullOrEmpty(wsdlUri))
                throw new Exception("Web Service Not Found");

            var xmlreader = new XmlTextReader(wsdlUri);

            var descriptionImporter = BuildServiceDescriptionImporter(xmlreader);

            return CompileAssembly(descriptionImporter);
        }

        private static ServiceDescriptionImporter BuildServiceDescriptionImporter(XmlReader xmlreader)
        {
            if (!ServiceDescription.CanRead(xmlreader))
                throw new Exception("Invalid Web Service Description");

            var serviceDescription = ServiceDescription.Read(xmlreader);

            var descriptionImporter = new ServiceDescriptionImporter { ProtocolName = "Soap" };
            descriptionImporter.AddServiceDescription(serviceDescription, null, null);
            descriptionImporter.Style = ServiceDescriptionImportStyle.Client;
            descriptionImporter.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;

            return descriptionImporter;
        }

        private static Assembly CompileAssembly(ServiceDescriptionImporter descriptionImporter)
        {
            var codeNamespace = new CodeNamespace();
            var codeUnit = new CodeCompileUnit();

            codeUnit.Namespaces.Add(codeNamespace);

            var importWarnings = descriptionImporter.Import(codeNamespace, codeUnit);

            if (importWarnings != 0)
                throw new Exception("Invalid Web Service Description: " + importWarnings);

            var compiler = CodeDomProvider.CreateProvider("CSharp");
            var references = new[] { "System.Web.Services.dll", "System.Xml.dll" };

            var parameters = new CompilerParameters(references);
            var results = compiler.CompileAssemblyFromDom(parameters, codeUnit);

            if (results.Errors.Cast<CompilerError>().Any())
                throw new Exception("Compilation Error Creating Assembly");

            return results.CompiledAssembly;
        }
    }
}