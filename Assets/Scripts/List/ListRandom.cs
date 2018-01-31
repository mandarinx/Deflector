using JetBrains.Annotations;
using UnityEngine;

namespace HyperGames.Lib {

    [AddComponentMenu("Hyper Games/Lib/Object List/Random")]
    [RequireComponent(typeof(ObjectList))]
    public class ListRandom : MonoBehaviour {

        [SerializeField]
        private UnityObjectEvent onNext;
        private ObjectList       list;
        private int              counter;
        private int[]            sequence;

        private void OnEnable() {
            list = GetComponent<ObjectList>();
            counter = 0;
            sequence = new int[list.Count];
            ArrayUtils.FillSequence(sequence);
            ArrayUtils.Shuffle(sequence);
        }

        /// <summary>
        /// Fetches the next object in the list and passes it to
        /// the onNext event. Usually called by a UnityEvent.
        /// </summary>
        [UsedImplicitly]
        public void FetchNextObject() {
            onNext.Invoke(list.GetObject(sequence[counter]));
            ++counter;
            if (counter < list.Count) {
                return;
            }
            ArrayUtils.Shuffle(sequence);
            counter = 0;
        }
    }
}
