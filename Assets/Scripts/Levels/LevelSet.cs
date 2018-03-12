using System;
using UnityEngine;

namespace Deflector {
    [CreateAssetMenu(menuName = "Sets/Level", fileName = "LevelSet.asset")]
    [Serializable]
    public class LevelSet : Set<Level> {}
}
