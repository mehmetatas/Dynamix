using Dynamix.Builder;

namespace Dynamix.Expressions
{
    public interface IExpression
    {
        void Accept(ExpressionEmitVisitor emitVisitor);
    }
}