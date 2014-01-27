using Dynamix.Builder;
using Dynamix.Metadata;

namespace Dynamix.Expressions
{
    class InstantiateExpression : IExpression
    {
        internal readonly ITypeInfo Type;

        public InstantiateExpression(ITypeInfo type)
        {
            Type = type;
        }

        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            emitVisitor.Visit(this);
        }
    }
}
