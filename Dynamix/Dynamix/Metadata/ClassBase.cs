
namespace Dynamix.Metadata
{
    public abstract class ClassBase : Construct
    {
        protected ClassBase()
        {
            BaseType = StaticType.Object;
            InheritanceModifier = InheritanceModifier.Default;
        }

        public ITypeInfo BaseType { get; set; }
        public InheritanceModifier InheritanceModifier { get; set; }
    }
}
