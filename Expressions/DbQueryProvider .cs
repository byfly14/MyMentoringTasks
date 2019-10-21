using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace Expressions
{
    public class DbQueryProvider : QueryProvider
    {
        private readonly DbConnection _connection;

        public DbQueryProvider(DbConnection connection)
        {
            _connection = connection;
        }
        
        public override string GetQueryText(Expression expression)
        {
            return Translate(expression).CommandText;
        }

        public override object Execute(Expression expression)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = Translate(expression).CommandText;
            var reader = cmd.ExecuteReader();
            var elementType = TypeSystem.GetElementType(expression.Type);
            
            return Activator.CreateInstance(
                typeof(ObjectReader<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { reader },
                null);
        }

        private static TranslateResult Translate(Expression expression)
        {
            return new MyQueryTranslator().Translate(expression);
        }
    }
}
