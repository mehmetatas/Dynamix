#region using IParentChildList
using IFieldParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.Field>;
using IPropertyParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.Property>;
using IIndexerParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.Indexer>;
using IEventParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.Event>;
using IMethodParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.Method>;
using IConstructorParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.Constructor>;
using INestedClassParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedClass>;
using INestedDelegateParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedDelegate>;
using INestedEnumParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedEnum>;
using INestedInterfaceParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedInterface>;
using INestedStructParent = Dynamix.Utils.IParent<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedStruct>;

using IFields = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Field>;
using IProperties = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Property>;
using IIndexers = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Indexer>;
using IEvents = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Event>;
using IMethods = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Method>;
using IConstructors = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Constructor>;
using INestedClasses = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedClass>;
using INestedDelegates = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedDelegate>;
using INestedEnums = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedEnum>;
using INestedInterfaces = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedInterface>;
using INestedStructs = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedStruct>;

using Fields = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Field>;
using Propertys = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Property>;
using Indexers = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Indexer>;
using Events = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Event>;
using Methods = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Method>;
using Constructors = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.Constructor>;
using NestedClasss = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedClass>;
using NestedDelegates = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedDelegate>;
using NestedEnums = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedEnum>;
using NestedInterfaces = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedInterface>;
using NestedStructs = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Construct, Dynamix.Metadata.NestedStruct>;
#endregion

namespace Dynamix.Metadata
{
    public abstract class Construct : ImplementorBase,
        IFieldParent, IPropertyParent, IIndexerParent, IEventParent, IMethodParent, IConstructorParent,
        INestedClassParent, INestedDelegateParent, INestedEnumParent, INestedInterfaceParent, INestedStructParent
    {
        protected Construct()
        {
            Fields = new Fields(this);
            Properties = new Propertys(this);
            Indexers = new Indexers(this);
            Events = new Events(this);
            Methods = new Methods(this);
            Constructors = new Constructors(this);
            NestedClasses = new NestedClasss(this);
            NestedDelegates = new NestedDelegates(this);
            NestedEnums = new NestedEnums(this);
            NestedInterfaces = new NestedInterfaces(this);
            NestedStructs = new NestedStructs(this);
        }

        public IFields Fields { get; private set; }
        public IProperties Properties { get; private set; }
        public IIndexers Indexers { get; private set; }
        public IEvents Events { get; private set; }
        public IMethods Methods { get; private set; }
        public IConstructors Constructors { get; private set; }
        public INestedClasses NestedClasses { get; private set; }
        public INestedDelegates NestedDelegates { get; private set; }
        public INestedEnums NestedEnums { get; private set; }
        public INestedInterfaces NestedInterfaces { get; private set; }
        public INestedStructs NestedStructs { get; private set; }

        IFields IFieldParent.Children
        {
            get { return Fields; }
        }
        IProperties IPropertyParent.Children
        {
            get { return Properties; }
        }
        IIndexers IIndexerParent.Children
        {
            get { return Indexers; }
        }
        IEvents IEventParent.Children
        {
            get { return Events; }
        }
        IMethods IMethodParent.Children
        {
            get { return Methods; }
        }
        IConstructors IConstructorParent.Children
        {
            get { return Constructors; }
        }
        INestedClasses INestedClassParent.Children
        {
            get { return NestedClasses; }
        }
        INestedDelegates INestedDelegateParent.Children
        {
            get { return NestedDelegates; }
        }
        INestedEnums INestedEnumParent.Children
        {
            get { return NestedEnums; }
        }
        INestedInterfaces INestedInterfaceParent.Children
        {
            get { return NestedInterfaces; }
        }
        INestedStructs INestedStructParent.Children
        {
            get { return NestedStructs; }
        }
    }
}