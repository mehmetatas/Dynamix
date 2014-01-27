using Dynamix.Builder;

namespace Dynamix.Expressions
{
    class CallExpression : IExpression
    {
        internal readonly string LocalVariableName;
        internal readonly string MethodName;

        internal CallExpression(string local, string method)
        {
            LocalVariableName = local;
            MethodName = method;
        }

        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            emitVisitor.Visit(this);
        }
    }
}
