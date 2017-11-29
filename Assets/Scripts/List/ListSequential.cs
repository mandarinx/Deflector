using UnityEngine;

namespace HyperGames.Lib {
    
    [AddComponentMenu("Hyper Games/Lib/Object List/Sequential")]
    [RequireComponent(typeof(ObjectList))]
    public class ListSequential : MonoBehaviour {

        [SerializeField]
        private UnityObjectEvent onNext;
        private ObjectList       list;
        private int              counter;
        
        private void OnEnable() {
            list = GetComponent<ObjectList>();
            counter = 0;
        }
    
        public void FetchNextObject() {
            onNext.Invoke(list.GetObject(counter));
            ++counter;
            counter %= list.Count;
        }
    }
}
