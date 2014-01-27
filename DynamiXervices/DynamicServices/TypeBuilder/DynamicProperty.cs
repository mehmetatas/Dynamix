using System;
using System.Collections.Generic;

namespace Taga.DynamicServices.TypeBuilder
{
    class DynamicProperty
    {
        public DynamicProperty()
        {
            Attributes = new List<Type>();
        }

        public Type Type { get; set; }
        public string Name { get; set; }

        public List<Type> Attributes { get; set; }
    }
}
