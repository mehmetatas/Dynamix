using System.Reflection.Emit;
using Dynamix.Metadata;
using Method = Dynamix.Metadata.Method;

namespace Dynamix.Builder
{
    class MethodBodyBuilder
    {
        private readonly MethodBody _methodBody;

        internal MethodBodyBuilder(MethodBody methodBody)
        {
            _methodBody = methodBody;
        }

        internal void Build(ILGenerator il)
        {
            var emitVisitor = new ExpressionEmitVisitor(il, _methodBody.Parent as Method);
            foreach (var expression in _methodBody.Expressions)
                expression.Accept(emitVisitor);
        }
    }
}
