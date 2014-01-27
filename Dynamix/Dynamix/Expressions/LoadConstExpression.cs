using System;
using Dynamix.Builder;

namespace Dynamix.Expressions
{
    class LoadConstExpression : IExpression
    {
        internal readonly object Value;
        internal readonly Type Type;

        public LoadConstExpression(object val, Type type)
        {
            Value = val;
            Type = type;
        }

        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            emitVisitor.Visit(this);
        }
    }
}
