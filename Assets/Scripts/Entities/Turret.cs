using UnityEngine;
using System.Collections;
using GameEvents;
using PowerTools;

namespace Deflector {
    public class Turret : MonoBehaviour {

        [SerializeField]
        private SpriteAnim      anim;
        [SerializeField]
        private GameObjectEvent onDespawn;
        [SerializeField]
        private Vector3Event    onFire;

        private void OnEnable() {
            StartCoroutine(Fire());
        }

        private void OnDisable() {
            StopAllCoroutines();
        }

        private IEnumerator Fire() {
            anim.Play(anim.Clip);
            yield return new WaitForSeconds(1.8f);
            onFire.Invoke(transform.position);
            yield return new WaitForSeconds(0.25f);
            onDespawn.Invoke(gameObject);
        }
    }
}
