using System;
using UnityEngine;

namespace Deflector {
    [CreateAssetMenu(menuName = "Sets/Spawn Points", fileName = "SpawnPointSet.asset")]
    [Serializable]
    public class SpawnPointSet : Set<SpawnPoint> {}
}
