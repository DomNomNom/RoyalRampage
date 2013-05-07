using UnityEngine;
using System.Collections;



public class ScreenShake : MonoBehaviour {

//    public camera : Camera; // set this via inspector
    public float shakeAmount = 0.2f;
    public float shakeDuration = 0.3f;

    private float remainingDuration = 0.0f;

    void Start () { }


    // Update is called once per frame
    void Update () {
        if (remainingDuration > 0) {
            transform.localPosition += Random.insideUnitSphere * remainingDuration * shakeAmount;
            remainingDuration -= Time.deltaTime;
//            Debug.Log("Shake dat");
        }
    }

    public void shake() {
        remainingDuration = shakeDuration;
    }
}
