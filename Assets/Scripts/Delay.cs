using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Delay : MonoBehaviour {

    public float delay;
    public UnityEvent onDelayed;
    
    public void StartDelay() {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown() {
        yield return new WaitForSeconds(delay);
        onDelayed.Invoke();
    }
}
