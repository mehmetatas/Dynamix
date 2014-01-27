#region using IParentChildList
using IInterfaceEventParent = Dynamix.Utils.IParent<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceEvent>;
using IInterfaceIndexerParent = Dynamix.Utils.IParent<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceIndexer>;
using IInterfaceMethodParent = Dynamix.Utils.IParent<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceMethod>;
using IInterfacePropertyParent = Dynamix.Utils.IParent<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceProperty>;

using IInterfaceEvents = Dynamix.Utils.IParentChildList<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceEvent>;
using IInterfaceIndexers = Dynamix.Utils.IParentChildList<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceIndexer>;
using IInterfaceMethods = Dynamix.Utils.IParentChildList<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceMethod>;
using IInterfaceProperties = Dynamix.Utils.IParentChildList<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceProperty>;

using InterfaceEvents = Dynamix.Utils.ParentChildList<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceEvent>;
using InterfaceIndexers = Dynamix.Utils.ParentChildList<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceIndexer>;
using InterfaceMethods = Dynamix.Utils.ParentChildList<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceMethod>;
using InterfaceProperties = Dynamix.Utils.ParentChildList<Dynamix.Metadata.InterfaceBase, Dynamix.Metadata.InterfaceProperty>;
#endregion

namespace Dynamix.Metadata
{
    public abstract class InterfaceBase : ImplementorBase,
        IInterfaceEventParent,
        IInterfaceIndexerParent,
        IInterfaceMethodParent,
        IInterfacePropertyParent
    {
        protected InterfaceBase()
        {
            Events = new InterfaceEvents(this);
            Indexers = new InterfaceIndexers(this);
            Methods = new InterfaceMethods(this);
            Properties = new InterfaceProperties(this);
        }

        public IInterfaceEvents Events { get; private set; }
        public IInterfaceIndexers Indexers { get; private set; }
        public IInterfaceMethods Methods { get; private set; }
        public IInterfaceProperties Properties { get; private set; }

        IInterfaceEvents IInterfaceEventParent.Children
        {
            get { return Events; }
        }
        IInterfaceIndexers IInterfaceIndexerParent.Children
        {
            get { return Indexers; }
        }
        IInterfaceMethods IInterfaceMethodParent.Children
        {
            get { return Methods; }
        }
        IInterfaceProperties IInterfacePropertyParent.Children
        {
            get { return Properties; }
        }
    }
}
