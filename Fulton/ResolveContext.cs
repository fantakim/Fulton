using Fulton.Exceptions;
using System;
using System.Collections.Generic;

namespace Fulton
{
    /// <summary>
    /// 가져오기 컨텍스트
    /// </summary>
    public sealed class ResolveContext
    {
        private readonly IList<string> currentKeysStack = new List<string>();

        internal bool Contains(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            return currentKeysStack.Contains(key);
        }

        internal void Push(string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            if (currentKeysStack.Contains(key))
                throw new CircularReferenceException(string.Format("순환 의존성 예외 발생 key: '{0}'", key));

            currentKeysStack.Add(key);
        }

        internal void Pop(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            currentKeysStack.Remove(key);
        }
    }
}
