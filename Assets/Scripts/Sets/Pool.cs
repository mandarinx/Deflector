using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public abstract class Pool<T> : ScriptableObject where T : class, new() {
    
    // THIS IS REALLY NOT A POOL
    // NEEDS INIT SIZE, INIT OF INSTANCE ++
    
    public IntEvent          onItemSpawned;
    public IntEvent          onItemDespawned;
    
    private readonly List<T> instances = new List<T>();

    public int Count => instances.Count;
    public T this[int i] => instances[i];

    public void Spawn() {
        T instance = new T();
        instances.Add(instance);
        onItemSpawned?.Invoke(instances.Count - 1);
    }

    public void Despawn(T item) {
        int i = instances.IndexOf(item);
        if (i < 0) {
            return;
        }
        onItemDespawned?.Invoke(i);
        instances.Remove(item);
        item = null;
    }

    public void DespawnAll() {
        for (int i = 0; i < instances.Count; ++i) {
            instances[i] = null;
        }
        instances.Clear();
    }
}
