using System;
using System.Collections.Generic;

namespace Deflector {
	// T must have a parameterless constructor
    public class ObjectPool<T> where T : class {

        public delegate                 void InstanceDelegate(T instance);

        private List<T>                 deactives;
        private List<T>                 actives;
        private readonly int            max;

        public Func<T>                  OnInstantiate;
        public InstanceDelegate         OnInstantiated;
        public InstanceDelegate         OnSpawned;
        public InstanceDelegate         OnWillSpawn;
        public InstanceDelegate         OnDespawned;
        public InstanceDelegate         OnWillDespawn;

		public int capacity {
            get { return max < 0 ? int.MaxValue : max; }
        }

        public int numItems {
            get { return actives.Count + deactives.Count; }
        }

        public int numSpawned {
            get { return actives.Count; }
        }

		/// <summary>
        /// Constructs an object pool of specified type, and allows adjusting the flexibility of the pool
        /// </summary>
        /// <param name="size">A number of slots to preallocate.</param>
        /// <param name="grow">Whether or not the pool should dynamically add new instances.</param>
        /// <returns>An ObjectPool of specified type.</returns>
        public ObjectPool(int size, bool grow = false) {
            max = grow ? -1 : size;
            actives = new List<T>(size);
            deactives = new List<T>(size);
		    OnInstantiate = () => (T)Activator.CreateInstance(typeof(T));
		    OnInstantiated = t => { };
		    OnSpawned = t => { };
		    OnWillSpawn = t => { };
		    OnDespawned = t => { };
		    OnWillDespawn = t => { };
		}

        /// <summary>
        /// Fills the pool with instances of the specified type.
        /// </summary>
        /// <returns>Nothing.</returns>
        public void Fill() {
            while (deactives.Count < deactives.Capacity) {
                AddInstance();
            }
        }

        /// <summary>
        /// Retrieve an object from the pool.
        /// </summary>
        /// <param name="instance">A reference to hold the returned instance.</param>
        /// <param name="onPostSpawn">Delegate for handling post spawn modifications of the instance.</param>
        /// <returns>True when the pool has an available instance, and false when it doesn't.</returns>
        public bool Spawn(out T instance) {
            if (!GetInstance(out instance)) {
                return false;
            }

            OnWillSpawn(instance);

            IPoolable poolable = instance as IPoolable;
            if (poolable != null) {
                poolable.OnEnable();
            }

            OnSpawned(instance);
            return true;
        }

        /// <summary>
        /// Puts an instance back into the pool.
        /// </summary>
        /// <param name="instance">A reference to an instance previously retrieved from the pool.</param>
        public void Despawn(T instance) {
            int index = GetIndexOf(actives, instance);
            if (index < 0) {
                return;
            }

            OnWillDespawn(instance);

            IPoolable poolable = instance as IPoolable;
            if (poolable != null) {
                poolable.OnDisable();
            }

            OnDespawned(instance);

            deactives.Add(instance);
            actives.RemoveAt(index);
        }

        /// <summary>
        /// Despawn all active instances.
        /// </summary>
        public void Reset() {
            while (actives.Count > 0) {
                Despawn(actives[0]);
            }
        }

        /// <summary>
        /// Destroy the pool and all instances. A pool needs to be reinitialized to be reused after being destroyed.
        /// </summary>
        public void Destroy() {
            DestroyInstances(deactives);
            DestroyInstances(actives);

            deactives.Clear();
            deactives = null;

            actives.Clear();
            actives = null;
        }

        public T GetInstance(int n) {
            return n < actives.Count ? actives[n] : default(T);
        }

        public override string ToString() {
            return "ObjectPool<" + typeof(T) + "> " +
                "Actives: " + actives.Count + "/" + actives.Capacity + " " +
                "Deactives: " + deactives.Count + "/" + deactives.Capacity + " " +
                "Grow: " + (max < 0 ? "true" : "false");
        }

        // Instantiates a new instance and calls any post init delegates.
        private void AddInstance() {
            T instance = OnInstantiate();
            deactives.Add(instance);

            IPoolable poolable = instance as IPoolable;
            if (poolable != null) {
                poolable.OnDisable();
            }

            OnInstantiated(instance);
        }

        // Retrieves an instance from the pool. If no deactives are available,
        // and are allowed to grow, instantiates a new instance.
        private bool GetInstance(out T instance) {
            if (deactives.Count == 0) {
                if (max < 0) {
                    AddInstance();
                } else {
                    instance = null;
                    return false;
                }
            }

            instance = deactives[0];
            actives.Add(instance);
            deactives.RemoveAt(0);
            return true;
        }

        // Destroys all instances in a given List
        private static void DestroyInstances(List<T> list) {
            int l = list.Count;
            for (int i=0; i<l; i++) {
                IPoolable poolable = list[i] as IPoolable;
                if (poolable != null) {
                    poolable.OnDestroy();
                }
                list[i] = null;
            }
        }

        // Get the index of an instance.
        private static int GetIndexOf(List<T> list, T instance) {
            if (list.Count == 0) {
                return -1;
            }

            int l = list.Count;
            for (int i=0; i<l; i++) {
                if (list[i].Equals(instance)) {
                    return i;
                }
            }

            return -1;
        }
	}
}
