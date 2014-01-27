using System.Collections.Generic;
using Dynamix.Builder;
using Dynamix.Expressions;

namespace Dynamix.Metadata
{
    public class MethodBody
    {
        internal MethodBody(MemberWithBody parent)
        {
            _parent = parent;
            _expressions = new List<IExpression>();
        }

        private readonly IList<IExpression> _expressions;
        internal IEnumerable<IExpression> Expressions
        {
            get { return _expressions; }
        } 

        private readonly MemberWithBody _parent;
        internal MemberWithBody Parent
        {
            get { return _parent; }
        }
        
        private MethodBodyBuilder _builder;
        internal MethodBodyBuilder Builder
        {
            get { return _builder ?? (_builder = new MethodBodyBuilder(this)); }
        }

        public void AddExpression(IExpression expression)
        {
            _expressions.Add(expression);
        }

        public bool IsEmpty
        {
            get { return _expressions.Count == 0; }
        }
    }
}