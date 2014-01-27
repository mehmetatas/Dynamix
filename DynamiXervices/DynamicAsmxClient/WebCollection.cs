using System.Collections;
using System.Collections.Generic;

namespace DynamicWebServiceClient
{
    internal abstract class WebCollection<T> : ICollection<T>
    {
        private readonly ICollection<T> _methods = new List<T>();

        internal abstract T this[string name] { get; }

        internal void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void Add(T item)
        {
            _methods.Add(item);
        }

        public void Clear()
        {
            _methods.Clear();
        }

        public bool Contains(T item)
        {
            return _methods.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _methods.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _methods.Count; }
        }

        public bool IsReadOnly
        {
            get { return _methods.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return _methods.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _methods.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _methods.GetEnumerator();
        }
    }
}
