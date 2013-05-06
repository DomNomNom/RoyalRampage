using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Rigidbody))]

public class ScreenremainingDuration : MonoBehaviour {

//    public camera : Camera; // set this via inspector
    public float shakeAmount = 0.7f;
    public float shakeDuration = 0.3f;

    private float remainingDuration = 0.0f;

    void Start () { }


    // Update is called once per frame
    void Update () {
        if (remainingDuration > 0) {
            transform.localPosition = Random.insideUnitSphere * remainingDuration * shakeAmount;
            remainingDuration -= Time.deltaTime;
        }
    }


    public void Shake() {
        remainingDuration = shakeDuration;
    }
}
