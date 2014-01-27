
namespace Dynamix.Metadata
{
    public abstract class NamedElement : Element
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
