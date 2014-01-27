using Reflection = System.Reflection;
using Dynamix.Builder;
#region using IParentChildList
using IClassParent = Dynamix.Utils.IParent<Dynamix.Metadata.Assembly, Dynamix.Metadata.Class>;
using IDelegateParent = Dynamix.Utils.IParent<Dynamix.Metadata.Assembly, Dynamix.Metadata.Delegate>;
using IInterfaceParent = Dynamix.Utils.IParent<Dynamix.Metadata.Assembly, Dynamix.Metadata.Interface>;
using IEnumParent = Dynamix.Utils.IParent<Dynamix.Metadata.Assembly, Dynamix.Metadata.Enum>;
using IStructParent = Dynamix.Utils.IParent<Dynamix.Metadata.Assembly, Dynamix.Metadata.Struct>;

using IClasses = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Assembly, Dynamix.Metadata.Class>;
using IInterfaces = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Assembly, Dynamix.Metadata.Interface>;
using IDelegates = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Assembly, Dynamix.Metadata.Delegate>;
using IEnums = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Assembly, Dynamix.Metadata.Enum>;
using IStructs = Dynamix.Utils.IParentChildList<Dynamix.Metadata.Assembly, Dynamix.Metadata.Struct>;

using Classes = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Assembly, Dynamix.Metadata.Class>;
using Interfaces = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Assembly, Dynamix.Metadata.Interface>;
using Delegates = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Assembly, Dynamix.Metadata.Delegate>;
using Enums = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Assembly, Dynamix.Metadata.Enum>;
using Structs = Dynamix.Utils.ParentChildList<Dynamix.Metadata.Assembly, Dynamix.Metadata.Struct>;
#endregion

namespace Dynamix.Metadata
{
    public class Assembly : NamedElement, IClassParent, IInterfaceParent, IDelegateParent, IEnumParent, IStructParent
    {
        public Assembly()
        {
            Classes = new Classes(this);
            Interfaces = new Interfaces(this);
            Delegates = new Delegates(this);
            Enums = new Enums(this);
            Structs = new Structs(this);
        }

        private AssemblyBuilder _builder;
        internal AssemblyBuilder Builder
        {
            get { return _builder ?? (_builder = new AssemblyBuilder(this)); }
        }

        public IClasses Classes { get; private set; }
        public IInterfaces Interfaces { get; private set; }
        public IDelegates Delegates { get; private set; }
        public IEnums Enums { get; private set; }
        public IStructs Structs { get; private set; }

        IClasses IClassParent.Children
        {
            get { return Classes; }
        }
        IInterfaces IInterfaceParent.Children
        {
            get { return Interfaces; }
        }
        IDelegates IDelegateParent.Children
        {
            get { return Delegates; }
        }
        IEnums IEnumParent.Children
        {
            get { return Enums; }
        }
        IStructs IStructParent.Children
        {
            get { return Structs; }
        }

        public Reflection.Assembly Compile()
        {
            return Compiler.Compile(this);
        }
    }
}
