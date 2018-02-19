using JetBrains.Annotations;
using UnityEngine;

namespace LunchGame01 {
    public class BloodController : MonoBehaviour {

        [SerializeField]
        private Sprite[]         blood;
        [SerializeField]
        private int              numSplats;
        [SerializeField]
        private float            radius;
        [SerializeField]
        private int              sortingOrder;

        private SpriteRenderer[] renderers;

        private void Awake() {
            renderers = new SpriteRenderer[blood.Length];
            for (int i = 0; i < renderers.Length; ++i) {
                GameObject renderer = new GameObject($"BloodRenderer_{i:00}");
                renderer.transform.SetParent(transform);
                renderer.transform.localPosition = Vector3.zero;
                renderer.SetActive(false);
                renderers[i] = renderer.AddComponent<SpriteRenderer>();
                renderers[i].sortingOrder = sortingOrder;
                renderers[i].sprite = blood[i];
            }
        }

        [UsedImplicitly]
        public void SpawnAt(Vector3 position) {
            ArrayUtils.Shuffle(renderers);
            for (int i = 0; i < numSplats; ++i) {
                Vector2 splatPos = Random.insideUnitCircle * radius;
                renderers[i].transform.position = position + (Vector3) splatPos;
                renderers[i].gameObject.SetActive(true);
            }
        }

        [UsedImplicitly]
        public void DespawnAll() {
            for (int i = 0; i < renderers.Length; ++i) {
                renderers[i].gameObject.SetActive(false);
            }
        }
    }
}
