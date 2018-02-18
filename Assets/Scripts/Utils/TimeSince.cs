using UnityEngine;

namespace LunchGame01 {
    public struct TimeSince {

        private float time;

        public static implicit operator float(TimeSince ts) {
            return Time.time - ts.time;
        }

        public static implicit operator TimeSince(float ts) {
            return new TimeSince { time = Time.time - ts };
        }
    }
}
