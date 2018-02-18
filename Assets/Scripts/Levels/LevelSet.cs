using System;
using UnityEngine;

namespace LunchGame01 {
    [CreateAssetMenu(menuName = "Sets/Level", fileName = "LevelSet.asset")]
    [Serializable]
    public class LevelSet : Set<Level> {}
}
