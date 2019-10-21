using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Expressions
{
    public class Query<T> : IOrderedQueryable<T>
    {
        private readonly QueryProvider _provider;
        private readonly Expression _expression;

        public Query(QueryProvider provider)
        {
            if (provider != null)
            {
                _provider = provider;
                _expression = Expression.Constant(this);
            }
            else
            {
                throw new ArgumentNullException(nameof(provider));
            }
        }

        public Query(QueryProvider provider, Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException(nameof(expression));
            }
            
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _expression = expression;
        }

        Expression IQueryable.Expression 
            => _expression;
        Type IQueryable.ElementType 
            => typeof(T);
        IQueryProvider IQueryable.Provider 
            => _provider;
        public IEnumerator<T> GetEnumerator() 
            => ((IEnumerable<T>)_provider.Execute(_expression)).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() 
            => ((IEnumerable)_provider.Execute(_expression)).GetEnumerator();
        public override string ToString() 
            => _provider.GetQueryText(_expression);
    }
}
