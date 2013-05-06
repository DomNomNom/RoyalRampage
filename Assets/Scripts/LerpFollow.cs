using UnityEngine;
using System.Collections;


public class LerpFollow : MonoBehaviour {
    public Transform target;
    public float damping = 1;

    // Use this for initialization
    void Start () {}

    // Update is called once per frame
    void Update () {
        transform.localPosition = Vector3.Lerp(transform.localPosition, target.localPosition, damping * Time.deltaTime);
    }
}
