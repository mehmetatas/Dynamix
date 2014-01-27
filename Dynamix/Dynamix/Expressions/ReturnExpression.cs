using Dynamix.Builder;

namespace Dynamix.Expressions
{
    class ReturnExpression : IExpression
    {
        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            emitVisitor.Visit(this);
        }
    }
}
