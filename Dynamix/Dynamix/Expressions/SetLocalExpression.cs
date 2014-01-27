using Dynamix.Builder;

namespace Dynamix.Expressions
{
    class SetLocalExpression : IExpression
    {
        internal readonly string LocalVariableName;

        public SetLocalExpression(string name)
        {
            LocalVariableName = name;
        }

        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            emitVisitor.Visit(this);
        }
    }
}
