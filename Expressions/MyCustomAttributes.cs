using System;

namespace Expressions
{
    public class DbColumnAttribute : Attribute
    {
        public string Name { get; }
        public DbColumnAttribute(string name)
        {
            this.Name = name;
        }
    }

    public class DbTableAttribute : Attribute
    {
        public string Name { get; }
        public DbTableAttribute(string name)
        {
            this.Name = name;
        }
    }
}
