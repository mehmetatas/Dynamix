using System;
using System.Collections.Generic;

namespace Taga.DynamicServices.TypeBuilder
{
    class DynamicType
    {
        public DynamicType()
        {
            Properties = new List<DynamicProperty>();
            Attributes = new List<Type>();
        }

        public string Name { get; set; }
        public string Namespace { get; set; }
        public List<DynamicProperty> Properties { get; set; }
        public List<Type> Attributes { get; set; }

        public string FullName
        {
            get
            {
                return String.IsNullOrWhiteSpace(Namespace)
                       ? Name
                       : Namespace + "." + Name;
            }
            set
            {
                var lastIndexOfDot = value.LastIndexOf('.');
                if (lastIndexOfDot < 0)
                {
                    Name = value;
                    Namespace = String.Empty;
                }
                else
                {
                    Name = value.Substring(lastIndexOfDot + 1);
                    Namespace = value.Substring(0, lastIndexOfDot);
                }
            }
        }
    }
}