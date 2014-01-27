
namespace Dynamix.Metadata
{
    public abstract class PropertyBase : PolymorphicMethod
    {
        protected PropertyBase()
        {
            Attribute = PolymorphicMemberAttribute.Default;
        }
        
        public ITypeInfo Type { get; set; }
    }
}
