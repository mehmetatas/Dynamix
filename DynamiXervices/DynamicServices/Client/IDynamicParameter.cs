using System;

namespace Taga.DynamicServices.Client
{
    public interface IDynamicParameter
    {
        Type Type { get; }
        string Name { get; }

        object Value { get; set; }
    }
}
