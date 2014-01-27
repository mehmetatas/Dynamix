using System.Collections.Generic;
using Dynamix.Builder;

namespace Dynamix.Expressions
{
    abstract class CompositeExpression : IExpression
    {
        private readonly IList<IExpression> _expressions;

        protected CompositeExpression()
        {
            _expressions = new List<IExpression>();
        }

        protected void Add(IExpression expression)
        {
            _expressions.Add(expression);
        }

        public void Accept(ExpressionEmitVisitor emitVisitor)
        {
            foreach (var expression in _expressions)
                expression.Accept(emitVisitor);
        }
    }
}