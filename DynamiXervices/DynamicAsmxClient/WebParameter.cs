using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DynamicWebServiceClient
{
    internal class WebParameter
    {
        private readonly Type _type;
        private readonly string _name;

        private object _value;
        private List<WebParameter> _children;

        internal WebParameter(string name, Type type)
        {
            _name = name;
            _type = type;
        }

        internal string Name
        {
            get { return _name; }
        }

        internal bool IsSystemType
        {
            get { return _type.IsValueType || _type == typeof(string); }
        }

        internal Type Type
        {
            get { return _type; }
        }

        private bool IsNullable
        {
            get { return _type.IsGenericType && _type.GetGenericTypeDefinition() == typeof(Nullable<>); }
        }

        private Type RealType
        {
            get { return IsNullable ? _type.GetGenericArguments()[0] : _type; }
        }

        internal object Value
        {
            get
            {
                return _value ?? (_value = IsSystemType
                                               ? _type.GetDefaultValue()
                                               : _type.CreateInstance());
            }
            set
            {
                try
                {
                    _value = Convert.ChangeType(value, RealType);
                }
                catch
                {
                    _value = null;
                }
            }
        }

        internal IEnumerable<WebParameter> Children
        {
            get
            {
                if (_children == null)
                    LoadChildren();
                return _children;
            }
        }

        internal object ToObject()
        {
            var thisObj = Value;

            foreach (var child in Children)
            {
                var propInf = _type.GetProperty(child._name);
                propInf.SetValue(thisObj, child.ToObject());
            }

            return thisObj;
        }

        internal void Load(object obj)
        {
            Value = obj;

            foreach (var child in Children)
            {
                var propInf = _type.GetProperty(child._name);
                child.Load(propInf.GetValue(obj));
            }
        }

        private void LoadChildren()
        {
            _children = new List<WebParameter>();

            if (IsSystemType)
                return;

            _children.AddRange(_type.GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                   BindingFlags.GetProperty | BindingFlags.SetProperty)
                                    .Select(pi => new WebParameter(pi.Name, pi.PropertyType)));
        }

        public override string ToString()
        {
            return _name + " : " + RealType.Name;
        }
    }
}
