using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace Expressions
{
    internal class ObjectReader<T> : IEnumerable<T> where T : new ()
    {
        private Enumerator _enumerator;
        
        internal ObjectReader(DbDataReader reader)
        {
            _enumerator = new Enumerator(reader);
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            var e = _enumerator;
            
            if (e == null)
            {
                throw new InvalidOperationException("Cannot enumerate more than once");
            }

            _enumerator = null;
            
            return e;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private sealed class Enumerator : IEnumerator<T>
        {
            private readonly DbDataReader _reader;
            private readonly FieldInfo[] _fields;

            private bool _disposed;
            private int[] _fieldLookup;

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            internal Enumerator(DbDataReader reader)
            {
                _reader = reader;
                _fields = typeof(T).GetFields();
            }

            public bool MoveNext()
            {
                if (!_reader.Read())
                {
                    return false;
                }

                if (typeof(T).IsValueType && typeof(T).IsPrimitive)
                {
                    var instancePrimitive = _reader.GetFieldValue<T>(0);
                    Current = instancePrimitive;
                    return true;
                }

                if (_fieldLookup == null)
                {
                    InitFieldLookup();
                }

                object boxed = new T();

                for (int i = 0, n = _fields.Length; i < n; i++)
                {
                    var index = _fieldLookup[i];

                    if (index < 0)
                    {
                        continue;
                    }

                    var fi = _fields[i];

                    fi.SetValue(boxed,  _reader.GetValue(index));
                }


                Current = (T)boxed;
                    
                return true;

            }
            
            public void Reset()
            {
                
            }
            

            private void InitFieldLookup()
            {
                var map = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
                
                for (int i = 0, n = _reader.FieldCount; i < n; i++)
                {
                    map.Add(_reader.GetName(i), i);
                }

                _fieldLookup = new int[_fields.Length];

                for (int i = 0, n = _fields.Length; i < n; i++)
                {
                    _fieldLookup[i] = map.TryGetValue(_fields[i].Name, out var index) ? index : -1;
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool disposing)
            {
                if (_disposed)
                    return;

                if (disposing)
                {
                    _reader.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
