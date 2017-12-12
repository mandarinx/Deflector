using System.Collections.Generic;
using UnityEngine;

public abstract class Set<T> : ScriptableObject {

    private readonly List<T> list = new List<T>();

    public int Count => list.Count;

    public void Add(T item) {
        list.Add(item);
    }

    public void Remove(T item) {
        list.Remove(item);
    }

    public void RemoveAll() {
        list.Clear();
    }

    public T Get(int n) {
        return list[n];
    }
}
