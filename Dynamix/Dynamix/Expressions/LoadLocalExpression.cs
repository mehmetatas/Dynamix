using Dynamix.Builder;

namespace Dynamix.Expressions
{
    class LoadLocalExpression : IExpression
    {
        internal readonly string LocalVariableName;

        public LoadLocalExpression(string name)
        {
            LocalVariableName = name;
        }

        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            emitVisitor.Visit(this);
        }
    }
}
