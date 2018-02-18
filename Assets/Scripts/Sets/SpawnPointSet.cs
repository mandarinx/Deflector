using System;
using UnityEngine;

namespace LunchGame01 {
    [CreateAssetMenu(menuName = "Sets/Spawn Points", fileName = "SpawnPointSet.asset")]
    [Serializable]
    public class SpawnPointSet : Set<SpawnPoint> {}
}
