using System;
using UnityEngine;

namespace Deflector {
    [CreateAssetMenu(menuName = "Data/Level")]
    [Serializable]
    public class Level : ScriptableObject {

        [SerializeField]
        private string     scenePath;
        [SerializeField]
        private GameMode   gameMode;

        public string      ScenePath => scenePath;
        public GameMode    GameMode  => gameMode;
    }
}
