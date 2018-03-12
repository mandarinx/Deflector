using System;
using UnityEngine;

namespace Deflector {
    [Serializable]
    public class SpawnPoint : MonoBehaviour {

        [SerializeField]
        private SpawnPointSet set;

        private void Start() {
            set.Add(this);
        }
    }
}
