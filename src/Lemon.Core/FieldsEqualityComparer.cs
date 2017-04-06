using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lemon.Core
{
    public class FieldsEqualityComparer<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> _compareExecutor;

        private string[] _fieldsToCompare;

        public FieldsEqualityComparer(string [] fieldsToCompare)
        {
            _fieldsToCompare = fieldsToCompare;

            _compareExecutor = BuildCompareExecutor();
        }

        private Func<T, T, bool> BuildCompareExecutor()
        {
            ParameterExpression left = Expression.Parameter(typeof(T), "left");
            ParameterExpression right = Expression.Parameter(typeof(T), "right");

            var firstColumn = _fieldsToCompare.First();

            BinaryExpression equalExpression = Expression.Equal(
                Expression.Property(left, firstColumn), 
                Expression.Property(right, firstColumn));

            foreach(var column in _fieldsToCompare.Skip(1))
            {
                equalExpression = Expression.AndAlso(equalExpression, Expression.Equal(
                    Expression.Property(left, column),
                    Expression.Property(right, column)));
            }

            return Expression.Lambda<Func<T, T, bool>>(equalExpression, left, right).Compile();
        }

        public bool Equals(T x, T y)
        {
            return _compareExecutor(x, y);
        }

        public int GetHashCode(T obj)
        {
            throw new NotSupportedException();
        }
    }
}
