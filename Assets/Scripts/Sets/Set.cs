using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public abstract class Set<T> : ScriptableObject {

    private readonly List<T> list = new List<T>();

    public IntEvent          onItemAdded;
    public IntEvent          onItemRemoved;
    
    public int Count => list.Count;
    public T this[int i] => list[i];

    public void Add(T item) {
        list.Add(item);
        onItemAdded?.Invoke(list.Count - 1);
    }

    public void Remove(T item) {
        int i = list.IndexOf(item);
        if (i < 0) {
            return;
        }
        onItemRemoved?.Invoke(i);
        list.Remove(item);
    }

    public void RemoveAll() {
        list.Clear();
    }
}
