using UnityEngine;

namespace HyperGames.Lib {
    
    [AddComponentMenu("Hyper Games/Lib/Object List/Object List")]
    public class ObjectList : MonoBehaviour {

        [SerializeField]
        private Object[]         objects;
        [SerializeField]
        private UnityObjectEvent onGetObject;

        public int Count => objects.Length;
        
        public Object GetObject(int i) {
            return objects[i];
        }
        
        public void DispatchObject(int i) {
            onGetObject.Invoke(objects[i]);
        }
    }
}
