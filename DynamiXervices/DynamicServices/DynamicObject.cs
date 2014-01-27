using System;
using System.Linq;
using System.Reflection;

namespace Taga.DynamicServices
{
    public class DynamicObject
    {
        private BindingFlags _commonBindingFlags = BindingFlags.Instance | BindingFlags.Public;

        public Type ObjectType { get; private set; }

        public object ObjectInstance { get; private set; }

        public BindingFlags BindingFlags
        {
            get { return _commonBindingFlags; }
            set { _commonBindingFlags = value; }
        }

        public DynamicObject(Object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            ObjectInstance = obj;
            ObjectType = obj.GetType();
        }

        public DynamicObject(Type objType)
        {
            if (objType == null)
                throw new ArgumentNullException("objType");

            ObjectType = objType;
        }

        public void CallConstructor()
        {
            ObjectInstance = ObjectType.CreateInstance();
        }

        public void CallConstructor(Type[] paramTypes, object[] paramValues)
        {
            var ctor = ObjectType.GetConstructor(paramTypes);
            if (ctor == null)
                throw new InvalidOperationException("The constructor matching the specified parameter types is not found.");

            ObjectInstance = ctor.Invoke(paramValues);
        }

        public object GetProperty(string property)
        {
            return Get(BindingFlags.GetProperty, property);
        }

        public void SetProperty(string property, object value, bool initializeNullReferences = false)
        {
            Set(BindingFlags.SetProperty, property, value, initializeNullReferences);

            try
            {
                // TODO: daha akýllý bir çözüm bulunmalý
                Set(BindingFlags.SetProperty, property + "Specified", value != null, initializeNullReferences);
            }
            catch
            {
            }
        }

        public object GetField(string field)
        {
            return Get(BindingFlags.GetField, field);
        }

        public void SetField(string field, object value, bool initializeNullReferences = false)
        {
            Set(BindingFlags.SetField, field, value, initializeNullReferences);
        }

        public object CallMethod(string method, params object[] parameters)
        {
            return ObjectType.InvokeMember(method, BindingFlags.InvokeMethod | _commonBindingFlags, null, ObjectInstance, parameters);
        }

        public object CallMethod(string method, Type[] types, object[] parameters)
        {
            if (types.Length != parameters.Length)
                throw new ArgumentException("The type for each parameter values must be specified.");

            var mi = ObjectType.GetMethod(method, types);
            if (mi == null)
                throw new ApplicationException(String.Format("The method {0} is not found.", method));

            return mi.Invoke(ObjectInstance, _commonBindingFlags, null, parameters, null);
        }

        private object Get(BindingFlags getFlag, string memberName, bool initializeNullReferences = false)
        {
            if (!memberName.Contains("."))
                return ObjectType.InvokeMember(memberName, getFlag | _commonBindingFlags, null, ObjectInstance, null);

            var obj = ObjectInstance;
            var children = memberName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var lastChild = ObjectType.Name;
            foreach (var child in children)
            {
                if (obj == null)
                    throw new NullReferenceException(String.Format("Child member {0} is null for {1}.{2}", lastChild, ObjectType.Name, memberName));

                var childObj = obj.GetType().InvokeMember(child, getFlag | _commonBindingFlags, null, obj, null);

                if (childObj == null && initializeNullReferences)
                    childObj = InitializeChild(InvertGetSetFlag(getFlag), obj, child);

                obj = childObj;
                lastChild = child;
            }
            return obj;
        }

        private static object InitializeChild(BindingFlags setFlag, object obj, string child)
        {
            var memberInfo = obj.GetType().GetMember(child).First();
            var type = GetMemberType(memberInfo);
            var childObj = type.CreateInstance();
            new DynamicObject(obj).Set(setFlag, child, childObj, false);
            return childObj;
        }

        private void Set(BindingFlags setFlag, string memberName, object value, bool initializeNullReferences)
        {
            if (!memberName.Contains("."))
            {
                ObjectType.InvokeMember(memberName, setFlag | _commonBindingFlags, null, ObjectInstance, new[] { value });
                return;
            }

            var lastDot = memberName.LastIndexOf('.');
            var parentMemberName = memberName.Substring(0, lastDot);

            var parentObj = Get(InvertGetSetFlag(setFlag), parentMemberName, initializeNullReferences);

            if (parentObj == null && initializeNullReferences)
                parentObj = InitializeChild(setFlag, ObjectInstance, parentMemberName);

            if (parentObj == null)
                throw new NullReferenceException(String.Format("Child member {0} is null for {1}.{2}", parentMemberName, ObjectType.Name, memberName));

            var member = memberName.Substring(lastDot + 1);
            parentObj.GetType().InvokeMember(member, setFlag | _commonBindingFlags, null, parentObj, new[] { value });
        }

        private static Type GetMemberType(MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo)
                return (memberInfo as PropertyInfo).PropertyType;

            if(memberInfo is FieldInfo)
                return (memberInfo as FieldInfo).FieldType;

            if (memberInfo is MethodInfo)
                return (memberInfo as MethodInfo).ReturnType;

            if (memberInfo is ConstructorInfo)
                return (memberInfo as ConstructorInfo).DeclaringType;

            if (memberInfo is EventInfo)
                return (memberInfo as EventInfo).EventHandlerType;
            
            if (memberInfo is Type)
                return memberInfo as Type;

            throw new ApplicationException("Unknow sub type for MemberInfo: " + memberInfo.GetType());
        }

        private static BindingFlags InvertGetSetFlag(BindingFlags flag)
        {
            if (flag == BindingFlags.SetField)
                return BindingFlags.GetField;
            
            if (flag == BindingFlags.GetField)
                return BindingFlags.SetField;

            if (flag == BindingFlags.SetProperty)
                return BindingFlags.GetProperty;

            if (flag == BindingFlags.GetProperty)
                return BindingFlags.SetProperty;

            throw new InvalidOperationException("BindingFlag in not a Get/Set flag: " + flag);
        }
    }
}
