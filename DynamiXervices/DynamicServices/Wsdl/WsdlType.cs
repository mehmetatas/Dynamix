using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Taga.DynamicServices.TypeBuilder;

namespace Taga.DynamicServices.Wsdl
{
    public class WsdlType : WsdlElement
    {
        [XmlAttribute("namespace")]
        public string Namespace { get; set; }

        [XmlElement("property")]
        public List<WsdlProperty> Properties { get; set; }

        internal Type DeclaredType
        {
            get { return ContractBuilder.GetDeclaredType(); }
        }

        internal Type BuildType()
        {
            return Type = ContractBuilder.BuildContractType();
        }

        private DynamicTypeBuilder _contractBuilder;
        private DynamicTypeBuilder ContractBuilder
        {
            get { return _contractBuilder ?? (_contractBuilder = DynamicModule.Instance.GetBuilder(Poco)); }
        }

        private DynamicType _poco;
        internal DynamicType Poco
        {
            get
            {
                if (_poco == null)
                {
                    _poco = new DynamicType { FullName = FullTypeName };
                    _poco.Attributes.Add(typeof(DataContractAttribute));

                    foreach (var wsdlProperty in Properties)
                    {
                        var pocoProp = new DynamicProperty {Name = wsdlProperty.Name};
                        pocoProp.Attributes.Add(typeof(DataMemberAttribute));
                        _poco.Properties.Add(pocoProp);
                    }
                }
                return _poco;
            }
        }
    }
}
