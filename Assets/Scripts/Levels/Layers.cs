using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace Deflector {
    public class Layers : MonoBehaviour {

        private Dictionary<string, Tilemap> layers;

        private void Awake() {
            layers = new Dictionary<string, Tilemap>();

            for (int i = 0; i < transform.childCount; ++i) {
                Transform child = transform.GetChild(i);
                Tilemap tilemap = child.GetComponent<Tilemap>();
                if (tilemap == null) {
                    continue;
                }

                layers.Add(child.name, tilemap);
            }
        }

        public Tilemap GetLayer(string layerName) {
            return layers[layerName];
        }
    }
}
