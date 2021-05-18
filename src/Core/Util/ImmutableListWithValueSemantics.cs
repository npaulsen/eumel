using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Immutable
{
    public class ImmutableListWithValueSemantics<T> : IImmutableList<T>, IEquatable<IImmutableList<T>>
    {
        private readonly IImmutableList<T> _list;

        public ImmutableListWithValueSemantics(IImmutableList<T> list) => _list = list;

        public static ImmutableListWithValueSemantics<T> Empty
            => Enumerable.Empty<T>().ToImmutableList().WithValueSemantics();

        #region IImmutableList implementation
        public T this[int index] => _list[index];

        public int Count => _list.Count;

        public IImmutableList<T> Add(T value) => _list.Add(value).WithValueSemantics();
        public IImmutableList<T> AddRange(IEnumerable<T> items) => _list.AddRange(items).WithValueSemantics();
        public IImmutableList<T> Clear() => _list.Clear().WithValueSemantics();
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        public int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) => _list.IndexOf(item, index, count, equalityComparer);
        public IImmutableList<T> Insert(int index, T element) => _list.Insert(index, element).WithValueSemantics();
        public IImmutableList<T> InsertRange(int index, IEnumerable<T> items) => _list.InsertRange(index, items).WithValueSemantics();
        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) => _list.LastIndexOf(item, index, count, equalityComparer);
        public IImmutableList<T> Remove(T value, IEqualityComparer<T> equalityComparer) => _list.Remove(value, equalityComparer).WithValueSemantics();
        public IImmutableList<T> RemoveAll(Predicate<T> match) => _list.RemoveAll(match).WithValueSemantics();
        public IImmutableList<T> RemoveAt(int index) => _list.RemoveAt(index).WithValueSemantics();
        public IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) => _list.RemoveRange(items, equalityComparer).WithValueSemantics();
        public IImmutableList<T> RemoveRange(int index, int count) => _list.RemoveRange(index, count).WithValueSemantics();
        public IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) => _list.Replace(oldValue, newValue, equalityComparer).WithValueSemantics();
        public IImmutableList<T> SetItem(int index, T value) => _list.SetItem(index, value);
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
        #endregion

        public override bool Equals(object obj) => Equals(obj as IImmutableList<T>);
        public bool Equals(IImmutableList<T> other) => this.SequenceEqual(other ?? ImmutableList<T>.Empty);
        public override int GetHashCode()
        {
            unchecked
            {
                return this.Aggregate(19, (h, i) => h * 19 + i.GetHashCode());
            }
        }
    }

    public static class Ex
    {
        public static ImmutableListWithValueSemantics<T> WithValueSemantics<T>(this IImmutableList<T> list) => new ImmutableListWithValueSemantics<T>(list);
    }
}