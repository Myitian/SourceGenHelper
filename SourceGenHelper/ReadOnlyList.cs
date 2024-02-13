using System.Collections;
using System.Collections.Generic;

namespace Myitian.SourceGenHelper
{
    public class ReadOnlyList<T>(IEnumerable<T> collection) : IReadOnlyList<T>
    {
        private readonly List<T> values = new(collection);
        public T this[int index] => values[index];
        public int Count => values.Count;
        public IEnumerator<T> GetEnumerator() => values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}