using System;
using System.Collections;
using GameEvents;
using UnityEngine;

public class Moveable : MonoBehaviour {

    public AnimationCurve  positionOverTime;
    public float           moveLength;
    public float           moveDuration;
    public Vector3Event    onOccupyCoord;
    public Vector3Event    onFreeCoord;

    private readonly int[] validAngles = { 0, 2, 4, 6 };

    private void Start() {
        onOccupyCoord.Invoke(transform.position);
    }

    public void Hit(int angleIndex) {
        if (Array.IndexOf(validAngles, angleIndex) < 0) {
            return;
        }
        StartCoroutine(Move(angleIndex));
    }

    private IEnumerator Move(int angleIndex) {
        Vector2 length = Angles.GetDirection(angleIndex) * moveLength;
        Vector2 startPos = transform.position;
        float startTime = Time.time;
        while (Time.time - startTime < moveDuration) {
            float dt = (Time.time - startTime) / moveDuration;
            float pos = positionOverTime.Evaluate(dt);
            transform.position = startPos + (length * pos);
            yield return null;
        }
        transform.position = startPos + length;
        
        onFreeCoord.Invoke(startPos);
        onOccupyCoord.Invoke(transform.position);
    }
}
