using Dynamix.Builder;
using Dynamix.Metadata;

namespace Dynamix.Expressions
{
    class DeclareLocalExpression : IExpression
    {
        internal readonly ITypeInfo Type;
        internal readonly string Name;

        public DeclareLocalExpression(ITypeInfo type, string name)
        {
            Type = type;
            Name = name;
        }

        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            emitVisitor.Visit(this);
        }
    }
}
