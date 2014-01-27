using Dynamix.Metadata;

namespace Dynamix.Expressions
{
    class NewLocalExpression: CompositeExpression
    {
        public NewLocalExpression(ITypeInfo type, string name)
        {
            Add(Expression.DeclareLocal(type, name));
            Add(Expression.Instantiate(type));
            Add(Expression.SetLocal(name));
        }
    }
}
