using UnityEngine;

namespace Deflector {
    public class InputController : MonoBehaviour {

        public static bool btnHit       => Input.GetButtonUp("Hit");
        public static bool btnUp        => Input.GetButtonUp("Up");
        public static bool btnDown      => Input.GetButtonUp("Down");
        public static bool btnRight     => Input.GetButtonUp("Right");
        public static bool btnLeft      => Input.GetButtonUp("Left");

        public static bool dirUp        => Input.GetAxisRaw("Vertical") > 0f;
        public static bool dirDown      => Input.GetAxisRaw("Vertical") < 0f;
        public static bool dirRight     => Input.GetAxisRaw("Horizontal") > 0f;
        public static bool dirLeft      => Input.GetAxisRaw("Horizontal") < 0f;

        public static uint dirUpUint    => (uint)(Input.GetAxisRaw("Vertical") > 0f ? 1 : 0);
        public static uint dirDownUint  => (uint)(Input.GetAxisRaw("Vertical") < 0f ? 1 : 0);
        public static uint dirRightUint => (uint)(Input.GetAxisRaw("Horizontal") > 0f ? 1 : 0);
        public static uint dirLeftUint  => (uint)(Input.GetAxisRaw("Horizontal") < 0f ? 1 : 0);

        public static bool anyKey       => btnHit || dirUp || dirRight || dirDown || dirLeft;

        public static uint inputMask {
            get {
                uint mask = 0;
                mask |= dirUpUint << 0;
                mask |= dirRightUint << 1;
                mask |= dirDownUint << 2;
                mask |= dirLeftUint << 3;
//            mask |= (uint)((Input.GetKey(KeyCode.UpArrow)    ? 1 : 0) << 0);
//            mask |= (uint)((Input.GetKey(KeyCode.RightArrow) ? 1 : 0) << 1);
//            mask |= (uint)((Input.GetKey(KeyCode.DownArrow)  ? 1 : 0) << 2);
//            mask |= (uint)((Input.GetKey(KeyCode.LeftArrow)  ? 1 : 0) << 3);
                return mask;
            }
        }
    }
}
