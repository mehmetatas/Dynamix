using Dynamix.Builder;
using Dynamix.Metadata;

namespace Dynamix.Expressions
{
    internal class EmptyMethodBodyExpression : IExpression
    {
        internal readonly ITypeInfo ReturnType;

        internal EmptyMethodBodyExpression(ITypeInfo returnType)
        {
            ReturnType = returnType;
        }

        public void Accept(ExpressionEmitVisitor expressionVisitor)
        {
            expressionVisitor.Visit(this);
        }
    }
}
