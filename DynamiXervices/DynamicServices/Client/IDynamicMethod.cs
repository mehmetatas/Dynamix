using System;
using System.Collections.Generic;

namespace Taga.DynamicServices.Client
{
    public interface IDynamicMethod
    {
        string Name { get; }
        Type ReturnType { get; }

        IEnumerable<IDynamicParameter> Parameters { get; }

        IDynamicParameter GetParameter(string parameterName);
        object Invoke();
    }
}
