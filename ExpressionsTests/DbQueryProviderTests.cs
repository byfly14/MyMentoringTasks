using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Expressions;
using NUnit.Framework;

namespace ExpressionsTests
{
    [TestFixture]
    public class DbQueryProviderTests
    {
        private SqlConnection _sqlConnection;
        private const string ConStr = @"Data Source=DESKTOP-GPEQNEN;Initial Catalog=ExpressionDatabase;Integrated Security=True";

        [Test]
        public void DbQueryProvider_WhereEqualMethod_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 2, Name = "Bill", Surname = "Gates"}
            };
            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Name == "Bill");
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereNotEqualMethod_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 1, Name = "Yan", Surname = "Kmita"},
                new Human {Id = 2, Name = "Bill", Surname = "Gates"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Id != 3);
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereGreaterThanMethod_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 3, Name = "Garry", Surname = "Kasparov"},
                new Human {Id = 2, Name = "Bill", Surname = "Gates"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Id > 1);
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereGreaterThanOrEqualMethod_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 3, Name = "Garry", Surname = "Kasparov"},
                new Human {Id = 2, Name = "Bill", Surname = "Gates"}
            };
            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Id >= 2);
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereLessThanMethod_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 1, Name = "Yan", Surname = "Kmita"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Id < 2);
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereLessThanOrEqualMethod_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 1, Name = "Yan", Surname = "Kmita"},
                new Human {Id = 2, Name = "Bill", Surname = "Gates"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Id <= 2);
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereMethodWithInvertedExpression_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 2, Name = "Bill", Surname = "Gates"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => 2 == c.Id);
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereMethodWithContains_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 2, Name = "Bill", Surname = "Gates"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Name.Contains("il"));
                var actualList = query.ToArray();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereMethodWithStartsWith_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 2, Name = "Bill", Surname = "Gates"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Surname.StartsWith("Ga"));
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereMethodWithEndsWith_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 2, Name = "Bill", Surname = "Gates"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Surname.EndsWith("es"));
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereMethodWithAndAlso_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id = 2, Name = "Bill", Surname = "Gates"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Surname.EndsWith("es") && c.Id == 2);
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereMethodWithOrAlso_ReturnCorrectSetOfData()
        {
            var expectedList = new List<Human>
            {
                new Human {Id =1, Name = "Yan", Surname = "Kmita"},
                new Human {Id =2, Name = "Bill", Surname = "Gates"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Surname.EndsWith("es") || c.Id == 1);
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_WhereValueThanSelect_ReturnCorrectSetOfData()
        {
            var expectedList = new List<int>
            {
                1, 2
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Where(c => c.Surname.EndsWith("es") || c.Id == 1).Select(c => c.Id);
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_SelectPrimitiveValueType_ReturnCorrectSetOfData()
        {
            var expectedList = new List<int>
            {
                1, 2, 3
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Select(c => c.Id);
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_SelectStructValueType_ReturnCorrectSetOfData()
        {
            var expectedList = new List<SelectHumanStruct>
            {
                new SelectHumanStruct {Name = "Yan", Surname = "Kmita"},
                new SelectHumanStruct {Name = "Bill", Surname = "Gates"},
                new SelectHumanStruct {Name = "Garry", Surname = "Kasparov"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Select(c => new SelectHumanStruct { Name = c.Name, Surname = c.Surname });
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_SelectReferenceType_ReturnCorrectSetOfData()
        {
            var expectedList = new List<SelectHuman>
            {
                new SelectHuman {Name = "Yan", Surname = "Kmita"},
                new SelectHuman {Name = "Bill", Surname = "Gates"},
                new SelectHuman {Name = "Garry", Surname = "Kasparov"}
            };

            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var query = db.Humans.Select(c => new SelectHuman { Name = c.Name, Surname = c.Surname });
                var actualList = query.ToList();

                CollectionAssert.AreEquivalent(expectedList, actualList);
                Console.WriteLine(query);
            }
        }

        [Test]
        public void DbQueryProvider_UseNotImplementedMethod_ThrowsNotSupportedException()
        {
            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);

                var ex = Assert.Throws<NotSupportedException>(() =>
                {
                    var result = db.Humans.Any();
                });

                Assert.AreEqual("The method 'Any' is not supported", ex.Message);
            }
        }

        [Test]
        public void DbQueryProvider_ToStringMethod_ReturnCorrectStringQuery()
        {
            var expectedQuery = "SELECT * FROM HUMANS".ToLower();
            using (_sqlConnection = new SqlConnection(ConStr))
            {
                _sqlConnection.Open();
                var db = new DbContext(_sqlConnection);
                var actualQuery = db.Humans.ToString().ToLower();

                Assert.AreEqual(expectedQuery, actualQuery);
                Console.WriteLine(actualQuery);
            }
        }

    }

    [DbTable("Humans")]
    public class Human
    {
        [DbColumn("IdColumn")]
        public int Id;
        [DbColumn("NameColumn")]
        public string Name;
        [DbColumn("SurnameColumn")]
        public string Surname;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Human)obj;

            return Id == other.Id
                   && string.Equals(Name, other.Name)
                   && string.Equals(Surname, other.Surname);
        }
    }

    [DbTable("Humans")]
    public class SelectHuman
    {
        [DbColumn("NameColumn")]
        public string Name;
        [DbColumn("SurnameColumn")]
        public string Surname;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (SelectHuman)obj;

            return string.Equals(Name, other.Name)
                   && string.Equals(Surname, other.Surname);
        }
    }

    [DbTable("Humans")]
    public struct SelectHumanStruct
    {
        [DbColumn("NameColumn")]
        public string Name;
        [DbColumn("SurnameColumn")]
        public string Surname;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (SelectHumanStruct)obj;

            return string.Equals(Name, other.Name)
                   && string.Equals(Surname, other.Surname);
        }
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
