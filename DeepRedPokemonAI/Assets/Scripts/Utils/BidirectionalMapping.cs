using System;
using System.Collections.Generic;

namespace com.MAB.Utils
{
    public class BidirectionalMapping<T, K>
    {
        private readonly Dictionary<T, K> First;
        private readonly Dictionary<K, T> Second;

        public BidirectionalMapping()
        {
            First = new Dictionary<T, K>();
            Second = new Dictionary<K, T>();
        }

        public void Add(T first, K second)
        {
            First.Add(first, second);
            Second.Add(second, first);
        }

        public K this[T value]
        {
            get
            {
                if (First.ContainsKey(value))
                {
                    return First[value];
                }

                throw new ArgumentException(nameof(value));
            }
        }

        public T this[K value]
        {
            get
            {
                if (Second.ContainsKey(value))
                {
                    return Second[value];
                }

                throw new ArgumentException(nameof(value));
            }
        }
    }
}
