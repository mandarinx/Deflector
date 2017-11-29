using UnityEngine;

namespace HyperGames.Lib {
    
    [AddComponentMenu("Hyper Games/Lib/Object List/Ping Pong")]
    [RequireComponent(typeof(ObjectList))]
    public class ListPingPong : MonoBehaviour {

        [SerializeField]
        private UnityObjectEvent onNext;
        private ObjectList       list;
        private int              counter;
        private int              dir;
        
        private void OnEnable() {
            list = GetComponent<ObjectList>();
            counter = 0;
            dir = 1;
        }
    
        public void FetchNextObject() {
            onNext.Invoke(list.GetObject(counter));
            counter += dir;
            if (counter >= list.Count) {
                dir = -1;
                counter = list.Count - 2;
            }
            if (counter < 0) {
                dir = +1;
                counter = 1;
            }
        }
    }
}
