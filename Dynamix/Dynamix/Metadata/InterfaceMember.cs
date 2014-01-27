
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public abstract class InterfaceMember : MemberBase, IChild<InterfaceBase>
    {
        public override string ToString()
        {
            return Parent + "." + Name;
        }

        public abstract InterfaceBase Parent { get; set; }
    }
}
