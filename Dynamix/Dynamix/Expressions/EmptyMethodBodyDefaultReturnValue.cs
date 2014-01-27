using Dynamix.Builder;
using Dynamix.Metadata;

namespace Dynamix.Expressions
{
    class EmptyMethodBodyDefaultReturnValue : IExpression
    {
        internal readonly ITypeInfo ReturnType;

        public EmptyMethodBodyDefaultReturnValue(ITypeInfo typeInfo)
        {
            ReturnType = typeInfo;
        }

        public void Accept(ExpressionEmitVisitor expressionVisitor)
        {
            expressionVisitor.Visit(this);
        }
    }
}
