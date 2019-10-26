using System;
using System.Linq;
using System.Linq.Expressions;

namespace Expressions
{
    public abstract class QueryProvider : IQueryProvider
    {
        //protected QueryProvider()
        //{

        //}
        
        IQueryable<T> IQueryProvider.CreateQuery<T>(Expression expression)
        {
            return new Query<T>(this, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        T IQueryProvider.Execute<T>(Expression expression) => (T)Execute(expression);

        public abstract string GetQueryText(Expression expression);

        public abstract object Execute(Expression expression);
    }
}
