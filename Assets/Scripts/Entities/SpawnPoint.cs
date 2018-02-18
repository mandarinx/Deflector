using System;
using UnityEngine;

namespace LunchGame01 {
    [Serializable]
    public class SpawnPoint : MonoBehaviour {

        [SerializeField]
        private SpawnPointSet set;

        private void Start() {
            set.Add(this);
        }
    }
}
