
namespace Dynamix.Expressions
{
    class AddArgsExpression : CompositeExpression
    {
        internal AddArgsExpression(int argIndex1, int argIndex2)
        {
            Add(Expression.LoadArg(argIndex1));
            Add(Expression.LoadArg(argIndex2));
            Add(Expression.Add());
        }

        public AddArgsExpression(string arg1, string arg2)
        {
            Add(Expression.LoadArg(arg1));
            Add(Expression.LoadArg(arg2));
            Add(Expression.Add());
        }
    }
}
