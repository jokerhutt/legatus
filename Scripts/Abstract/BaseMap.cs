using System.Collections.Generic;

namespace Practice.Scripts.Abstract;

public abstract class BaseMap<T>
{
    protected Dictionary<string, T> Items = new();

    public void Add(string id, T item)
    {
        Items[id] = item;
    }

    public bool TryGet(string id, out T item)
    {
        return Items.TryGetValue(id, out item);
    }

    public T Get(string id)
    {
        return Items[id];
    }
    
    public IEnumerable<T> GetManyStrict(IEnumerable<string> ids)
    {
        foreach (var id in ids)
        {
            yield return Items[id];
        }
    }

    public IEnumerable<T> GetAll()
    {
        return Items.Values;
    }
}