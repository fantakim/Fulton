using System;
using System.Collections.Generic;

namespace Fulton
{
    /// <summary>
    /// 컨테이너
    /// </summary>
    public class Container : IContainer
    {
        private readonly IDictionary<string, Func<ResolveContext, object>> builders = new Dictionary<string, Func<ResolveContext, object>>();
        private readonly IDictionary<string, RegisteredObject> factoryBuilders = new Dictionary<string, RegisteredObject>();
        private readonly IDictionary<Type, string> typeFullNameCache = new Dictionary<Type, string>();
        private readonly object syncRoot = new object();
        private ResolveContext currentResolveContext;

        public Container()
        {
        }

        /// <summary>
        /// 등록
        /// </summary>
        /// <typeparam name="TDependency">의존T</typeparam>
        /// <param name="factory">인스턴스 생성 대리자</param>
        public void Register<TDependency>(Func<IContainer, TDependency> factory)
        {
            Register(typeof(TDependency), c => (factory(c) as object));
        }

        /// <summary>
        /// 등록
        /// </summary>
        /// <typeparam name="TDependency">의존T</typeparam>
        /// <typeparam name="TResolve">가져오기T</typeparam>
        public void Register<TDependency, TResolve>() where TResolve : TDependency
        {
            this.Register(typeof(TDependency), typeof(TResolve));
        }

        /// <summary>
        /// 등록
        /// </summary>
        /// <param name="dependency">의존 타입</param>
        /// <param name="resolve">가져오기 타입</param>
        public void Register(Type dependency, Type resolve)
        {
            if(dependency.ContainsGenericParameters)
                this.Register(dependency, null, resolve);
            else
                this.Register(dependency, CreateInstanceDelegateFactory.Create(resolve));
        }

        /// <summary>
        /// 등록
        /// </summary>
        /// <param name="dependency">의존 타입</param>
        /// <param name="factory">인스턴스 생성 대리자</param>
        /// <param name="resolve">가져오기 타입</param>
        private void Register(Type dependency, Func<IContainer, object> factory, Type resolve = null)
        {
            var key = GetTypeFullName(dependency);

            if (factoryBuilders.ContainsKey(key))
            {
                var message = string.Format("'{0}' 이미 등록된 타입 입니다.", dependency);
                throw new Exception(message);
            }
            
            RegisteredObject registeredObject;

            if (resolve != null)
                registeredObject = new RegisteredObject(this, dependency, resolve);
            else
                registeredObject = new RegisteredObject(this, dependency, factory);

            factoryBuilders[key] = registeredObject;
        }

        /// <summary>
        /// 가져오기
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>T</returns>
        public T Resolve<T>()
        {
            var name = GetTypeFullName(typeof(T));
            var arguments = typeof(T).IsGenericType
                                ? typeof(T).GetGenericArguments()
                                : Type.EmptyTypes;

            var resolveContext = currentResolveContext ?? new ResolveContext();

            return (T)Resolve(name, arguments, resolveContext);
        }

        /// <summary>
        /// 가져오기
        /// </summary>
        /// <param name="key">키</param>
        /// <param name="genericParameters">제너릭파라미터</param>
        /// <param name="context">가져오기 컨텍스트</param>
        /// <returns>타입</returns>
        public object Resolve(string key, Type[] genericParameters, ResolveContext context)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (context == null)
                throw new ArgumentNullException("context");

            lock(syncRoot)
            {
                currentResolveContext = context;

                context.Push(key);

                Func<ResolveContext, object> builder;
                if (!builders.TryGetValue(key, out builder))
                {
                    builder = CreateBuilder(key, genericParameters);
                    builders[key] = builder;
                }

                var result = builder(context);

                context.Pop(key);

                return result;
            }
        }

        /// <summary>
        /// 빌더 만들기
        /// </summary>
        /// <param name="key">키</param>
        /// <param name="genericParameters">제너릭파라미터</param>
        /// <returns>대리자</returns>
        private Func<ResolveContext, object> CreateBuilder(string key, Type[] genericParameters)
        {
            RegisteredObject factory;
            if (factoryBuilders.TryGetValue(key, out factory))
            {
                return factory.GetFactoryMethod(genericParameters);
            }

            var message = string.Format("'{0}' 타입은 등록되지 않았습니다.", key);
            throw new Exception(message);
        }

        /// <summary>
        /// 타입 풀네임 가져오기
        /// </summary>
        /// <param name="type">타입</param>
        /// <returns>풀네임</returns>
        private string GetTypeFullName(Type type)
        {
            string name;
            if (!typeFullNameCache.TryGetValue(type, out name))
            {
                name = type.AssemblyQualifiedName;
                typeFullNameCache[type] = name;
            }

            return name;
        }
    }
}
