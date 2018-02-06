using UnityEngine;

namespace mlib {

    [CreateAssetMenu(menuName = "Sets/AudioItem Set")]
    public class AudioItemSet : ScriptableObject {

        [SerializeField]
        private AudioItem[] audioItems = new AudioItem[0];
        [SerializeField]
        private float       weightSum;
        private int         next;

        private void OnEnable() {
            next = 0;
            weightSum = ComputeWeightSum(this);
        }

        public AudioItem GetRandom() {
			float rnd = Random.value * weightSum;

			for (int i = 0; i < audioItems.Length; ++i) {
				if (rnd < audioItems[i].Weight) {
					return audioItems[i];
				}

				rnd -= audioItems[i].Weight;
			}
			return audioItems[0];
		}

        public AudioItem GetNext() {
            AudioItem item = audioItems[next];
            next = (next + 1) % audioItems.Length;
            return item;
        }

        public static float ComputeWeightSum(AudioItemSet set) {
			float sum = 0;
			for (int i = 0 ; i < set.audioItems.Length; ++i) {
				sum += set.audioItems[i].Weight;
			}
			return sum;
        }
    }
}
