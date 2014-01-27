using Dynamix.Builder;

namespace Dynamix.Expressions
{
    class LoadArgByNameExpression : IExpression
    {
        internal readonly string ArgName;

        public LoadArgByNameExpression(string argName)
        {
            ArgName = argName;
        }

        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            emitVisitor.Visit(this);
        }
    }
}
