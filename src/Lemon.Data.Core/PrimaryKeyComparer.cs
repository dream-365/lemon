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
        private delegate int CompareExecutor(T left, T right);

        private CompareExecutor _executor;

        public PrimaryKeyComparer(string primaryKey)
        {
            var primaryKeyProp = typeof(T).GetProperty(primaryKey);

            var propType = primaryKeyProp.PropertyType;

            var iCompareInterface = propType.GetInterface("IComparable`1");

            if(iCompareInterface == null)
            {
                throw new ArgumentException("The primary key type must implemented the IComparable interface");
            }

            var compareMethod = iCompareInterface.GetMethod("CompareTo");

            ParameterExpression left = Expression.Parameter(typeof(T), "left");

            ParameterExpression right = Expression.Parameter(typeof(T), "right");

            var leftPrimaryKeyPropExpression = Expression.Property(left, primaryKey);
            var rightPrimaryKeyPropExpression = Expression.Property(right, primaryKey);

            var call = Expression.Call(leftPrimaryKeyPropExpression, compareMethod, new[] { rightPrimaryKeyPropExpression });

            _executor = Expression.Lambda<CompareExecutor>(call, left, right).Compile();
        }

        public override int Compare(T x, T y)
        {
            return _executor(x, y);
        }
    }
}
