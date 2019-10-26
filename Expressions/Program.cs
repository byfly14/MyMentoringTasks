using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Expressions
{
    internal class Program
    {
        private static void Main()
        {
            const string conStr = @"Data Source=DESKTOP-GPEQNEN;Initial Catalog=ExpressionDatabase;Integrated Security=True";

            using (var con = new SqlConnection(conStr))
            {
                con.Open();
                var db = new DbContext(con);

                var query = db.Humans/*.Where(c => c.Id == 2 || c.Name.Contains("n"))*/.Select(c => /*c.Id*/new SelectHumanStruct { Name = c.Name, Surname = c.Surname });

                Console.WriteLine("Query:\n{0}\n", query);

                foreach (var item in query)
                {
                    Console.WriteLine($"{item.Name}");
                }

                Console.ReadLine();
            }
        }

        public class Human
        {
            public int Id;
            public string Name;
            public string Surname;
        }


        public class SelectHuman
        {
            public string Name;
            public string Surname;
        }

        public struct SelectHumanStruct
        {
            public string Name;
            public string Surname;
        }

        public class DbContext
        {
            public Query<Human> Humans { get; set; }

            public DbContext(DbConnection connection)
            {
                QueryProvider provider = new DbQueryProvider(connection);
                Humans = new Query<Human>(provider);
            }
        }
    }
}
