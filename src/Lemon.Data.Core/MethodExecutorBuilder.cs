using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Lemon.Data.Core
{
    public class MethodExecutorBuilder
    {
        public delegate object MethodExecutor(object instance, object[] parameters);

        public static MethodExecutor Build(MethodInfo method)
        {
            ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "instance");
            ParameterExpression parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            List<Expression> parameters = new List<Expression>();

            ParameterInfo[] parameterInfos = method.GetParameters();

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo paramInfo = parameterInfos[i];
                BinaryExpression valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(valueObj, paramInfo.ParameterType);
                parameters.Add(valueCast);
            }

            UnaryExpression instanceCast = (!method.IsStatic) ? Expression.Convert(instanceParameter, method.ReflectedType) : null;

            MethodCallExpression methodCall = Expression.Call(instanceCast, method, parameters);

            Expression conversion = Expression.Convert(methodCall, typeof(object));

            var executor = Expression.Lambda<MethodExecutor>(conversion, instanceParameter, parametersParameter).Compile();

            return executor;
        }
    }
}
