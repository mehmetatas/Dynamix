
namespace Dynamix.Metadata
{
    public class Parameter : NamedElement
    {
        public static readonly Parameter[] EmptyParameters = new Parameter[0];

        public Parameter()
        {
            Attribute = ParameterAttribute.Default;
        }

        public ITypeInfo Type { get; set; }
        public ParameterAttribute Attribute { get; set; }

        public override string ToString()
        {
            return Type + " : " + Name;
        }
    }
}
