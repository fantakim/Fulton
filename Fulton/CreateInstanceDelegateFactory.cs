using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fulton
{
    /// <summary>
    /// 팩토리 대리자 생성
    /// </summary>
    public class CreateInstanceDelegateFactory
    {
        /// <summary>
        /// 생성
        /// </summary>
        /// <param name="type">타입</param>
        /// <returns>대리자</returns>
        public static Func<IContainer, object> Create(Type type)
        {
            ParameterExpression container = Expression.Parameter(typeof(IContainer), "container");
            NewExpression exp = BuildExpression(type, container);
            return Expression.Lambda<Func<IContainer, object>>(
                    exp,
                    new ParameterExpression[] { container }
                ).Compile();
        }

        /// <summary>
        /// 표현식
        /// </summary>
        /// <param name="type">타입</param>
        /// <param name="container">컨테이너</param>
        /// <returns></returns>
        private static NewExpression BuildExpression(Type type, ParameterExpression container)
        {
            if (!type.IsGenericTypeDefinition)
            {
                ConstructorInfo constructor = GetConstructorInfo(type);
                ParameterInfo[] parameters = constructor.GetParameters();

                List<Expression> arguments = new List<Expression>();

                foreach (var paramInfo in parameters)
                {
                    var p = Expression.Call(container, "Resolve", new Type[] { paramInfo.ParameterType },
                      new Expression[] { });
                    arguments.Add(p);
                }

                return Expression.New(constructor, arguments);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 생성자 가져오기
        /// </summary>
        /// <param name="type">타입</param>
        /// <returns>생성자 정보</returns>
        private static ConstructorInfo GetConstructorInfo(Type type)
        {
            var constructors = type.GetConstructors();
            var constructor = constructors
                               .OrderBy(c => c.GetParameters().Length)
                               .LastOrDefault();

            if (constructor == null)
                throw new ArgumentException(String.Format("The requested class {0} does not have a public constructor", type));

            return constructor;
        }
    }
}
