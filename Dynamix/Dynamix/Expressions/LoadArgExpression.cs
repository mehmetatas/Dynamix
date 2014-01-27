using Dynamix.Builder;

namespace Dynamix.Expressions
{
    class LoadArgExpression : IExpression
    {
        internal readonly int ArgIndex;

        internal LoadArgExpression(int argIndex)
        {
            ArgIndex = argIndex;
        }

        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            emitVisitor.Visit(this);
        }
    }
}
