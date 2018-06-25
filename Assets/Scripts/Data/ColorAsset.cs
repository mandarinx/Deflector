using UnityEngine;

namespace Deflector {

    [CreateAssetMenu(menuName = "Data/Color", fileName = "ColorAsset.asset")]
    public class ColorAsset : ScriptableObject {

        [SerializeField]
        private Color color;

        public Color Color => color;
    }
}
