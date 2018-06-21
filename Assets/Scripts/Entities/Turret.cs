using UnityEngine;
using System.Collections;
using GameEvents;

namespace Deflector {
    public class Turret : MonoBehaviour {

        [SerializeField]
        private SpriteAnimPlayer anim;
        [SerializeField]
        private GameObjectEvent  onSpawn;
        [SerializeField]
        private GameObjectEvent  onDespawn;
        [SerializeField]
        private Vector3Event     onFire;

        public void Activate() {
            StartCoroutine(Fire());
        }

        private void OnDisable() {
            StopAllCoroutines();
        }

        private IEnumerator Fire() {
            onSpawn.Invoke(gameObject);
            anim.Play();
            yield return new WaitForSeconds(1.8f);
            onFire.Invoke(transform.position);
            yield return new WaitForSeconds(0.25f);
            onDespawn.Invoke(gameObject);
        }
    }
}
