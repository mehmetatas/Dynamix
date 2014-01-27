using System;
using System.Collections.Generic;

namespace Taga.DynamicServices.Client
{
    public interface IDynamicService
    {
        Type Type { get; }
        string Name { get; }
        string Address { get; set; }
        IEnumerable<IDynamicMethod> Methods { get; }

        IDynamicMethod GetMethod(string methodName);
    }
}
