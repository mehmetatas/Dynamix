using Dynamix.Metadata;

namespace Dynamix.Expressions
{
    class SetNewLocalExpression : CompositeExpression
    {
        public SetNewLocalExpression(ITypeInfo type, string name)
        {
            Add(Expression.DeclareLocal(type, name));
            Add(Expression.SetLocal(name));
        }
    }
}
