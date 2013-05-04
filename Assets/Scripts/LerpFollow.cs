using UnityEngine;
using System.Collections;


public class LerpFollow : MonoBehaviour {
    public Transform target;
    public float damping = 1;

    // Use this for initialization
    void Start () {}

    // Update is called once per frame
    void Update () {
        transform.position = Vector3.Lerp(transform.position, target.position, damping * Time.deltaTime);
    }
}
