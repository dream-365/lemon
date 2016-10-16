using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Lemon.Data.Core
{
    public class PrimaryKeyComparer<T> : Comparer<T>
    {
        private Func<T, T, int> _executor;

        private string _primaryKey;

        public PrimaryKeyComparer(string primaryKey)
        {
            _primaryKey = primaryKey;

            _executor = BuildExecutor();
        }

        private Func<T, T, int> BuildExecutor()
        {
            var primaryKeyProp = typeof(T).GetProperty(_primaryKey);

            var propType = primaryKeyProp.PropertyType;

            var iCompareInterface = propType.GetInterface("IComparable`1");

            if (iCompareInterface == null)
            {
                throw new ArgumentException("The primary key type must implemented the IComparable interface");
            }

            var compareMethod = iCompareInterface.GetMethod("CompareTo");

            ParameterExpression left = Expression.Parameter(typeof(T), "left");

            ParameterExpression right = Expression.Parameter(typeof(T), "right");

            var leftPrimaryKeyPropExpression = Expression.Property(left, _primaryKey);
            var rightPrimaryKeyPropExpression = Expression.Property(right, _primaryKey);

            var call = Expression.Call(leftPrimaryKeyPropExpression, compareMethod, new[] { rightPrimaryKeyPropExpression });

            return Expression.Lambda<Func<T, T, int>>(call, left, right).Compile();
        }

        public override int Compare(T x, T y)
        {
            return _executor(x, y);
        }
    }
}
