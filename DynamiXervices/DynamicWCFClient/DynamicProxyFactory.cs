using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Web.Services.Discovery;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using WsdlNS = System.Web.Services.Description;

namespace Taga.DynamicServices.WCFClient
{
    class DynamicProxyFactory
    {
        internal const string DefaultNamespace = "http://tempuri.org/";
        private readonly DynamicProxyFactoryOptions _options;
        private readonly string _wsdlUri;

        private CodeCompileUnit _codeCompileUnit;
        private CodeDomProvider _codeDomProvider;
        private ServiceContractGenerator _contractGenerator;

        private ServiceEndpointCollection _endpoints;
        private Collection<MetadataSection> _metadataCollection;

        internal IEnumerable<MetadataSection> Metadata
        {
            get { return _metadataCollection; }
        }

        internal IEnumerable<ServiceEndpoint> Endpoints
        {
            get { return _endpoints; }
        }

        internal IEnumerable<Binding> Bindings { get; private set; }

        internal IEnumerable<ContractDescription> Contracts { get; private set; }

        internal Assembly ProxyAssembly { get; private set; }

        internal string ProxyCode { get; private set; }

        internal IEnumerable<MetadataConversionError> MetadataImportWarnings { get; private set; }

        internal IEnumerable<MetadataConversionError> CodeGenerationWarnings { get; private set; }

        internal IEnumerable<CompilerError> CompilationWarnings { get; private set; }

        public DynamicProxyFactory(string wsdlUri)
            : this(wsdlUri, new DynamicProxyFactoryOptions())
        {
        }

        public DynamicProxyFactory(string wsdlUri, DynamicProxyFactoryOptions options)
        {
            if (wsdlUri == null)
                throw new ArgumentNullException("wsdlUri");

            if (options == null)
                throw new ArgumentNullException("options");

            _wsdlUri = wsdlUri;
            _options = options;
        }

        internal void Init()
        {
            DownloadMetadata();
            ImportMetadata();
            CreateProxy();
            WriteCode();
            CompileProxy();
        }

        internal DynamicProxy CreateProxy(string contractName)
        {
            return CreateProxy(contractName, null);
        }

        internal DynamicProxy CreateProxy(ServiceEndpoint endpoint)
        {
            var contractType = GetContractType(endpoint.Contract.Name, endpoint.Contract.Namespace);

            var proxyType = GetProxyType(contractType);

            return new DynamicProxy(proxyType, endpoint.Binding, endpoint.Address);
        }

        internal DynamicProxy CreateProxy(string contractName, string contractNamespace)
        {
            var endpoint = GetEndpoint(contractName, contractNamespace);
            return CreateProxy(endpoint);
        }

        internal ServiceEndpoint GetEndpoint(string contractName)
        {
            return GetEndpoint(contractName, null);
        }

        internal ServiceEndpoint GetEndpoint(string contractName, string contractNamespace)
        {
            var matchingEndpoint = Endpoints.FirstOrDefault(endpoint =>
                ContractNameMatch(endpoint.Contract, contractName) &&
                ContractNsMatch(endpoint.Contract, contractNamespace));

            if (matchingEndpoint == null)
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.EndpointNotFound,
                    contractName, contractNamespace));

            return matchingEndpoint;
        }

        private void DownloadMetadata()
        {
            var disco = new DiscoveryClientProtocol
                {
                    AllowAutoRedirect = true,
                    UseDefaultCredentials = true
                };
            disco.DiscoverAny(_wsdlUri);
            disco.ResolveAll();

            var results = new Collection<MetadataSection>();
            foreach (var document in disco.Documents.Values)
            {
                AddDocumentToResults(document, results);
            }
            _metadataCollection = results;
        }

        private static void AddDocumentToResults(object document, Collection<MetadataSection> results)
        {
            var wsdl = document as WsdlNS.ServiceDescription;
            var schema = document as XmlSchema;
            var xmlDoc = document as XmlElement;

            if (wsdl != null)
            {
                results.Add(MetadataSection.CreateFromServiceDescription(wsdl));
            }
            else if (schema != null)
            {
                results.Add(MetadataSection.CreateFromSchema(schema));
            }
            else if (xmlDoc != null && xmlDoc.LocalName == "Policy")
            {
                results.Add(MetadataSection.CreateFromPolicy(xmlDoc, null));
            }
            else
            {
                var mexDoc = new MetadataSection();
                mexDoc.Metadata = document;
                results.Add(mexDoc);
            }
        }

        private void ImportMetadata()
        {
            _codeCompileUnit = new CodeCompileUnit();
            CreateCodeDomProvider();

            var importer = new WsdlImporter(new MetadataSet(_metadataCollection));
            AddStateForDataContractSerializerImport(importer);
            AddStateForXmlSerializerImport(importer);

            Bindings = importer.ImportAllBindings();
            Contracts = importer.ImportAllContracts();
            _endpoints = importer.ImportAllEndpoints();
            MetadataImportWarnings = importer.Errors;

            var success = MetadataImportWarnings == null ||
                          MetadataImportWarnings.All(error => error.IsWarning);

            if (success)
                return;

            throw new DynamicProxyException(Constants.ErrorMessages.ImportError)
            {
                MetadataImportErrors = MetadataImportWarnings
            };
        }

        private void AddStateForXmlSerializerImport(WsdlImporter importer)
        {
            var importOptions = new XmlSerializerImportOptions(_codeCompileUnit)
            {
                CodeProvider = _codeDomProvider,
                WebReferenceOptions = new WsdlNS.WebReferenceOptions
                {
                    CodeGenerationOptions = CodeGenerationOptions.GenerateProperties |
                                            CodeGenerationOptions.GenerateOrder
                }
            };

            importOptions.WebReferenceOptions.SchemaImporterExtensions.Add(typeof(TypedDataSetSchemaImporterExtension).AssemblyQualifiedName);
            importOptions.WebReferenceOptions.SchemaImporterExtensions.Add(typeof(DataSetSchemaImporterExtension).AssemblyQualifiedName);

            importer.State.Add(typeof(XmlSerializerImportOptions), importOptions);
        }

        private void AddStateForDataContractSerializerImport(WsdlImporter importer)
        {
            var xsdDataContractImporter = new XsdDataContractImporter(_codeCompileUnit)
            {
                Options = new ImportOptions
                {
                    ImportXmlType = _options.FormatMode == DynamicProxyFactoryOptions.FormatModeOptions.DataContractSerializer,
                    CodeProvider = _codeDomProvider
                }
            };

            importer.State.Add(typeof(XsdDataContractImporter), xsdDataContractImporter);

            foreach (var dcConverter in importer.WsdlImportExtensions.OfType<DataContractSerializerMessageContractImporter>())
                dcConverter.Enabled = _options.FormatMode != DynamicProxyFactoryOptions.FormatModeOptions.XmlSerializer;
        }

        private void CreateProxy()
        {
            CreateServiceContractGenerator();

            foreach (var contract in Contracts)
                _contractGenerator.GenerateServiceContractType(contract);

            var success = true;
            CodeGenerationWarnings = _contractGenerator.Errors;
            if (CodeGenerationWarnings != null)
                success = CodeGenerationWarnings.All(error => error.IsWarning);

            if (success)
                return;

            throw new DynamicProxyException(Constants.ErrorMessages.CodeGenerationError)
            {
                CodeGenerationErrors = CodeGenerationWarnings
            };
        }

        private void WriteCode()
        {
            using (var writer = new StringWriter())
            {
                var codeGenOptions = new CodeGeneratorOptions
                {
                    BracingStyle = "C"
                };

                _codeDomProvider.GenerateCodeFromCompileUnit(_codeCompileUnit, writer, codeGenOptions);

                writer.Flush();
                ProxyCode = writer.ToString();
            }

            if (_options.CodeModifier != null)
                ProxyCode = _options.CodeModifier(ProxyCode);
        }

        private void CompileProxy()
        {
            var compilerParams = new CompilerParameters { GenerateInMemory = true };

            AddAssemblyReference(typeof(ServiceContractAttribute).Assembly, compilerParams.ReferencedAssemblies);
            AddAssemblyReference(typeof(WsdlNS.ServiceDescription).Assembly, compilerParams.ReferencedAssemblies);
            AddAssemblyReference(typeof(DataContractAttribute).Assembly, compilerParams.ReferencedAssemblies);
            AddAssemblyReference(typeof(XmlElement).Assembly, compilerParams.ReferencedAssemblies);
            AddAssemblyReference(typeof(Uri).Assembly, compilerParams.ReferencedAssemblies);
            AddAssemblyReference(typeof(DataSet).Assembly, compilerParams.ReferencedAssemblies);

            var results = _codeDomProvider.CompileAssemblyFromSource(compilerParams, ProxyCode);

            if (results.Errors != null && results.Errors.HasErrors)
                throw new DynamicProxyException(Constants.ErrorMessages.CompilationError)
                {
                    CompilationErrors = ToEnumerable(results.Errors)
                };

            CompilationWarnings = ToEnumerable(results.Errors);
            //ProxyAssembly = Assembly.LoadFile(results.PathToAssembly);
            ProxyAssembly = results.CompiledAssembly;
        }

        private Type GetContractType(string contractName, string contractNamespace)
        {
            var contractType = (from type in ProxyAssembly.GetTypes()
                                where type.IsInterface
                                let attrs = type.GetCustomAttributes(typeof(ServiceContractAttribute), false)
                                where (attrs != null) && (attrs.Length != 0)
                                let scAttr = (ServiceContractAttribute)attrs[0]
                                let cName = GetContractName(type, scAttr.Name, scAttr.Namespace)
                                where cName.Name == contractName &&
                                      cName.Namespace == contractNamespace
                                select type).FirstOrDefault();

            if (contractType == null)
                throw new ArgumentException(Constants.ErrorMessages.UnknownContract);

            return contractType;
        }

        private Type GetProxyType(Type contractType)
        {
            var clientBaseType = typeof(ClientBase<>).MakeGenericType(contractType);

            var allTypes = ProxyAssembly.GetTypes();
            var proxyType = allTypes.FirstOrDefault(type =>
                                                    type.IsClass &&
                                                    contractType.IsAssignableFrom(type) &&
                                                    type.IsSubclassOf(clientBaseType));

            if (proxyType == null)
                throw new DynamicProxyException(String.Format(Constants.ErrorMessages.ProxyTypeNotFound, contractType.FullName));

            return proxyType;
        }

        private void CreateCodeDomProvider()
        {
            _codeDomProvider = CodeDomProvider.CreateProvider(_options.Language.ToString());
        }

        private void CreateServiceContractGenerator()
        {
            _contractGenerator = new ServiceContractGenerator(_codeCompileUnit);
            _contractGenerator.Options |= ServiceContractGenerationOptions.ClientClass;
        }

        internal static string ToString(IEnumerable<MetadataConversionError> importErrors)
        {
            if (importErrors == null)
                return null;

            var importErrStr = new StringBuilder();

            foreach (var error in importErrors)
                importErrStr.AppendLine(String.Format("{0} : {1}", error.IsWarning ? "Warning" : "Error", error.Message));

            return importErrStr.ToString();
        }

        internal static string ToString(IEnumerable<CompilerError> compilerErrors)
        {
            if (compilerErrors == null)
                return null;

            var builder = new StringBuilder();
            foreach (var error in compilerErrors)
                builder.AppendLine(error.ToString());

            return builder.ToString();
        }

        private static void AddAssemblyReference(Assembly referencedAssembly, StringCollection refAssemblies)
        {
            var path = Path.GetFullPath(referencedAssembly.Location);
            var name = Path.GetFileName(path);

            if (!refAssemblies.Contains(name) && !refAssemblies.Contains(path))
                refAssemblies.Add(path);
        }

        private static XmlQualifiedName GetContractName(Type contractType, string name, string ns)
        {
            if (String.IsNullOrEmpty(name))
                name = contractType.Name;

            ns = ns == null ? DefaultNamespace : Uri.EscapeUriString(ns);

            return new XmlQualifiedName(name, ns);
        }

        private static bool ContractNameMatch(ContractDescription cDesc, string name)
        {
            return (String.Compare(cDesc.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
        }

        private static bool ContractNsMatch(ContractDescription cDesc, string ns)
        {
            return (ns == null ||
                    String.Compare(cDesc.Namespace, ns, StringComparison.OrdinalIgnoreCase) == 0);
        }

        private static IEnumerable<CompilerError> ToEnumerable(CompilerErrorCollection collection)
        {
            return collection == null ? null : collection.Cast<CompilerError>();
        }
    }
}