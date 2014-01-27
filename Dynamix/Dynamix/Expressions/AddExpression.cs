using Dynamix.Builder;

namespace Dynamix.Expressions
{
    class AddExpression : IExpression
    {
        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            emitVisitor.Visit(this);
        }
    }
}
