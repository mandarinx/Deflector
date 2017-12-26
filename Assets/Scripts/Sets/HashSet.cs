using System.Collections;
using Generic = System.Collections.Generic;
using UnityEngine;

public abstract class HashSet<T> : ScriptableObject {

    private readonly Generic.HashSet<T> set = new Generic.HashSet<T>();

    public int Count => set.Count;

    private void OnEnable() {
        set.Clear();
    }

    public virtual void Add(T item) {
        set.Add(item);
    }

    public virtual void Remove(T item) {
        set.Remove(item);
    }

    public void RemoveAll() {
        set.Clear();
    }

    public IEnumerator GetEnumerator() {
        return set.GetEnumerator();
    }
}
