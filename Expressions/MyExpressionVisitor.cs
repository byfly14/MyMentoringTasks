using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions
{
    internal class MyQueryTranslator : ExpressionVisitor
    {
        private StringBuilder _sb;
        private ParameterExpression _row;

        internal string Translate(Expression expression)
        {
            _sb = new StringBuilder();
            _row = Expression.Parameter(typeof(ProjectionRow), "row");

            Visit(expression);
            return _sb.ToString();
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }

            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            switch (m.Method.Name)
            {
                case "Where":
                    {
                        _sb.Append("SELECT * FROM (");
                        Visit(m.Arguments[0]);
                        _sb.Append(") AS T WHERE ");
                        var lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                        Visit(lambda.Body);

                        return m;
                    }

                case "Select":
                    {
                        var lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                        var projection = new ColumnProjector().ProjectColumns(lambda.Body, _row);
                        _sb.Append("SELECT ");
                        _sb.Append(projection.Columns);
                        _sb.Append(" FROM (");
                        Visit(m.Arguments[0]);
                        _sb.Append(") AS T");

                        return m;
                    }

                case "Contains":
                    {
                        Visit(m.Object);
                        _sb.Append(" LIKE '%");
                        Visit(m.Arguments[0]);
                        _sb.Append("%'");
                        return m;
                    }

                case "StartsWith":
                    {
                        Visit(m.Object);
                        _sb.Append(" LIKE '");
                        Visit(m.Arguments[0]);
                        _sb.Append("%'");
                        return m;
                    }

                case "EndsWith":
                    {
                        Visit(m.Object);
                        _sb.Append(" LIKE '%");
                        Visit(m.Arguments[0]);
                        _sb.Append("'");
                        return m;
                    }

                default:
                    throw new NotSupportedException($"The method '{m.Method.Name}' is not supported");
            }
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            _sb.Append("(");
            Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    _sb.Append(" AND ");
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _sb.Append(" OR ");
                    break;

                case ExpressionType.Equal:
                    _sb.Append(" = ");
                    break;

                case ExpressionType.NotEqual:
                    _sb.Append(" <> ");
                    break;

                case ExpressionType.LessThan:
                    _sb.Append(" < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    _sb.Append(" <= ");
                    break;

                case ExpressionType.GreaterThan:
                    _sb.Append(" > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    _sb.Append(" >= ");
                    break;
            }

            Visit(b.Right);
            _sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value is IQueryable q)
            {
                _sb.Append("SELECT * FROM ");
                var attr = (DbTableAttribute)q.ElementType
                    .GetCustomAttributes(typeof(DbTableAttribute), true)
                    .FirstOrDefault();
                _sb.Append(attr != null ? attr.Name : q.ElementType.Name);
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.String:
                        {
                            var commandString = _sb.ToString();
                            var containsLike = commandString.Substring(commandString.Length - 7).Contains("LIKE '");
                            if (!containsLike)
                            {
                                _sb.Append("'");
                            }

                            _sb.Append(c.Value);

                            if (!containsLike)
                            {
                                _sb.Append("'");
                            }
                        }
                        break;

                    default:
                        _sb.Append(c.Value);
                        break;
                }
            }

            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression == null || m.Expression.NodeType != ExpressionType.Parameter)
            {
                throw new NotSupportedException($"The member '{m.Member.Name}' is not supported");
            }

            var attr = (DbColumnAttribute)m.Member
                .GetCustomAttributes(typeof(DbColumnAttribute), true)
                .FirstOrDefault();
            _sb.Append(attr != null ? attr.Name : m.Member.Name);
            return m;
        }
    }
}
