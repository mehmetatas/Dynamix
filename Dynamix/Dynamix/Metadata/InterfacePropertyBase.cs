
namespace Dynamix.Metadata
{
    public abstract class InterfacePropertyBase : InterfaceMember
    {
        public ReturnValue ReturnValue { get; set; }
        public bool AllowGet { get; set; }
        public bool AllowSet { get; set; }
    }
}
