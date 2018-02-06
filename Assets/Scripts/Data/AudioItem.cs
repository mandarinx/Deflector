using System;
using UnityEngine;

namespace mlib {

    [Serializable]
	public class AudioItem {

		[SerializeField]
		private AudioClip clip;
		[SerializeField]
		private float     weight     = GetDefaultWeight();
        [SerializeField]
        private Vector2   pitchRange = GetDefaultPitchRange();

        public float      Weight     => weight;
        public Vector2    PitchRange => pitchRange;
        public AudioClip  AudioClip  => clip;

        public static float GetDefaultWeight() {
            return 1f;
        }

        public static Vector2 GetDefaultPitchRange() {
            return Vector2.one;
        }
    }
}
