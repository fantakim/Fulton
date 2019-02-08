using System;

namespace Fulton
{
    /// <summary>
    /// 등록된 객체
    /// </summary>
    public class RegisteredObject
    {
        /// <summary>
        /// 컨테이너
        /// </summary>
        public IContainer Container { get; private set; }

        /// <summary>
        /// 의존 타입
        /// </summary>
        public Type Dependency { get; private set; }

        /// <summary>
        /// 가져오기 타입
        /// </summary>
        public Type Resolve { get; private set; }

        /// <summary>
        /// 팩토리 대리자
        /// </summary>
        public Func<IContainer, object> Factory;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="container">컨테이너</param>
        /// <param name="dependency">의존 타입</param>
        /// <param name="factory">팩토리 대리자</param>
        public RegisteredObject(IContainer container, Type dependency, Func<IContainer, object> factory)
        {
            this.Container = container;
            this.Dependency = dependency;
            this.Factory = factory;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="container">컨테이너</param>
        /// <param name="dependency">의존 타입</param>
        /// <param name="resolve">가져오기 타입</param>
        public RegisteredObject(IContainer container, Type dependency, Type resolve)
        {
            this.Container = container;
            this.Dependency = dependency;
            this.Resolve = resolve;
        }

        /// <summary>
        /// 인스턴스 가져오기
        /// </summary>
        /// <returns></returns>
        public object GetInstance()
        {
            return Factory(this.Container);
        }

        /// <summary>
        /// 팩토리 메소드 가져오기
        /// </summary>
        /// <param name="arguments">타입 아큐먼트</param>
        /// <returns>팩토리 대리자</returns>
        public Func<ResolveContext, object> GetFactoryMethod(params Type[] arguments)
        {
            Func<ResolveContext, object> ResolveObject = (r) =>
            {
                return GetInstance();
            };

            return ResolveObject;
        }
    }
}
