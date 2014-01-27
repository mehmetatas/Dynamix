
namespace Dynamix.Expressions
{
    class ReturnLocalExpression : CompositeExpression
    {
        public ReturnLocalExpression(string name)
        {
            Add(Expression.LoadLocal(name));
            Add(Expression.Return());
        }
    }
}
