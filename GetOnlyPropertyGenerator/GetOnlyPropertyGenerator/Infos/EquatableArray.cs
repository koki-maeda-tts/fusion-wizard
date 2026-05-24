using System.Collections;

namespace GetOnlyPropertyGenerator.Infos;

internal readonly struct EquatableArray<T>(T?[] items)
    : IEquatable<EquatableArray<T>>,
        IEnumerable<T?>
    where T : IEquatable<T>
{
    private readonly T?[] _items = items;

    public bool Equals(EquatableArray<T> other)
    {
        if (other._items is null && _items is null)
        {
            return true;
        }
        if (other._items is null || _items is null || other._items.Length != _items.Length)
        {
            return false;
        }
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] is null && other._items[i] is null)
            {
                continue;
            }
            if (_items[i] is null || other._items[i] is null)
            {
                return false;
            }
            if (!_items[i]!.Equals(other._items[i]!))
            {
                return false;
            }
        }
        return true;
    }

    public override bool Equals(object? obj)
    {
        return obj is EquatableArray<T> other && Equals(other);
    }

    public IEnumerator<T?> GetEnumerator()
    {
        foreach (var item in _items)
        {
            yield return item;
        }
    }

    public override int GetHashCode()
    {
        if (_items is null)
        {
            return 0;
        }
        unchecked
        {
            int hash = (int)2166136261;

            foreach (var item in _items)
            {
                hash ^= item?.GetHashCode() ?? 0;
                hash *= 16777619;
            }

            return hash;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
