using System;

namespace Dynamix.Expressions
{
    class MethodCallExpression : CompositeExpression
    {
        public MethodCallExpression(string local, string method, object[] args, Type[] argTypes)
        {
            Add(Expression.LoadLocal(local));
            for (var i = 0; i < args.Length; i++)
                Add(Expression.LoadConst(args[i], argTypes[i]));
            Add(Expression.Call(local, method));
        }
    }
}
