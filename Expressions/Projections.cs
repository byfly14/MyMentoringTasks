﻿using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Expressions
{
    public abstract class ProjectionRow
    {
        public abstract object GetValue(int index);
    }

    internal class ColumnProjection
    {
        internal string Columns;
        internal Expression Selector;
    }

    internal class ColumnProjector : ExpressionVisitor
    {
        private StringBuilder _sb;
        private int _iColumn;
        private ParameterExpression _row;
        private static MethodInfo _miGetValue;

        internal ColumnProjector()
        {
            if (_miGetValue == null)
            {
                _miGetValue = typeof(ProjectionRow).GetMethod("GetValue");
            }
        }

        internal ColumnProjection ProjectColumns(Expression expression, ParameterExpression row)
        {
            _sb = new StringBuilder();
            _row = row;
            
            var selector = Visit(expression);
            return new ColumnProjection
            {
                Columns = _sb.ToString(),
                Selector = selector
            };
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression == null || m.Expression.NodeType != ExpressionType.Parameter)
            {
                return base.VisitMember(m);
            }

            if (_sb.Length > 0)
            {
                _sb.Append(", ");
            }

            _sb.Append(m.Member.Name);

            return Expression.Convert(
                Expression.Call(_row, _miGetValue, Expression.Constant(_iColumn++)), m.Type);
        }
    }
}
