using System;
using UnityEngine;
using UnityEngine.Rendering;

// Should implement a pool to avoid creating new GOs all the time.
// Hide the pool in the static methods.

namespace Mandarin {

    public partial class GO {

        private GameObject current;

        static public GO Create(string name = "GameObject") {
            return new GO(name);
        }

        static public GO Create(PrimitiveType type) {
            return new GO(GameObject.CreatePrimitive(type));
        }

        static public GO Modify(Transform t) {
            return new GO(t.gameObject);
        }

        static public GO Modify(GameObject go) {
            return new GO(go);
        }

        static public GO Instantiate(GameObject prefab) {
            return new GO(GameObject.Instantiate(prefab));
        }

        public GO(string name = "GameObject") {
            current = new GameObject(name);
        }

        public GO(GameObject go) {
            current = go;
        }

        public GO SetName(string name) {
            current.name = name;
            return this;
        }

        public GO SetLayer(string name) {
            current.layer = LayerMask.NameToLayer(name);
            return this;
        }

        public GO SetParent(Transform parent, bool worldPositionStays = true) {
            current.transform.SetParent(parent, worldPositionStays);
            return this;
        }

        public GO SetParent(GO parent, bool worldPositionStays = true) {
            current.transform.SetParent(parent.transform, worldPositionStays);
            return this;
        }

        public GO SetAsLastSibling() {
            current.transform.SetAsLastSibling();
            return this;
        }

        public GO SetActive(bool active = true) {
            current.SetActive(active);
            return this;
        }

        public GO SetMesh(Mesh mesh) {
            GetSetComp<MeshFilter>().sharedMesh = mesh;
            return this;
        }

        public GO SetMaterial(Material mat) {
            GetSetComp<MeshRenderer>().material = mat;
            return this;
        }

        public GO SetSharedMaterial(Material mat) {
            GetSetComp<MeshRenderer>().sharedMaterial = mat;
            return this;
        }

        public GO ReceiveShadows(bool receiveShadows) {
            GetSetComp<MeshRenderer>().receiveShadows = receiveShadows;
            return this;
        }

        public GO CastShadows(ShadowCastingMode castShadows) {
            GetSetComp<MeshRenderer>().shadowCastingMode = castShadows;
            return this;
        }

        public GO SetScale(Vector3 scale) {
            current.transform.localScale = scale;
            return this;
        }

        public GO SetPosition(Vector3 position) {
            current.transform.position = position;
            return this;
        }

        public GO SetLocalPosition(Vector3 position) {
            current.transform.localPosition = position;
            return this;
        }

        public GO SetRotation(Quaternion quaternion, bool local = false) {
            if (local) {
                current.transform.localRotation = quaternion;
            } else {
                current.transform.rotation = quaternion;
            }
            return this;
        }

        public GO AddBoxCollider(Vector3 size, Vector3 center = default(Vector3)) {
            BoxCollider collider = GetSetComp<BoxCollider>();
            collider.size = size;
            collider.center = center;
            return this;
        }

        public GO AddComponent<T>(Action<T> callback = null) where T : Component {
            T component = current.AddComponent<T>();
            if (callback != null) {
                callback(component);
            }
            return this;
        }

        public GO AddComponent(Type type, Action<Component> callback = null) {
            Component component = current.AddComponent(type);
            if (callback != null) {
                callback(component);
            }
            return this;
        }

        public GO GetComponent<T>(Action<T> callback) where T : Component {
            T comp = current.GetComponent<T>();
            if (comp != null) {
                callback(comp);
            }
            return this;
        }

        public T GetComponent<T>() where T : Component {
            return current.GetComponent<T>();
        }

        public GO GetComponents<T>(Action<T> callback) where T : Component {
            T[] comps = current.GetComponents<T>();
            for (int i=0; i<comps.Length; i++) {
                callback(comps[i]);
            }
            return this;
        }

        public void Destroy() {
            GameObject.Destroy(current);
        }

        public void Destroy(float afterSeconds) {
            GameObject.Destroy(current, afterSeconds);
        }

        public GameObject Duplicate() {
            return GameObject.Instantiate(current);
        }

        private T GetSetComp<T>() where T : Component {
            T comp = current.GetComponent<T>();
            if (comp == null) {
                comp = current.AddComponent<T>();
            }
            return comp;
        }

        public GameObject gameObject {
            get { return current; }
        }

        public Transform transform {
            get { return current.transform; }
        }
    }
}
