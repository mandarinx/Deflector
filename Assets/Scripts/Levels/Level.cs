using System;
using UnityEngine;

namespace Deflector {
    [CreateAssetMenu(menuName = "Data/Level")]
    [Serializable]
    public class Level : ScriptableObject {

        [SerializeField]
        private string     scenePath;
        [SerializeField]
        private GameMode[] gameModes;

        public int         NumGameModes => gameModes.Length;
        public string      ScenePath    => scenePath;

        public GameMode GetGameMode(int n) {
            return gameModes[n];
        }
    }
}
