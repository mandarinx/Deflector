using UnityEngine;

public static class LayerMasks {

    public static bool LayerInMask(int mask, LayerMask layer) {
        return ((1 << mask) & layer.value) > 0;
    }

    public static string ToBitString(LayerMask mask) {
        return System.Convert.ToString(mask, 2).PadLeft(32, '0');
    }
}
