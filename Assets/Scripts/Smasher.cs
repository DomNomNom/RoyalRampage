using UnityEngine;
using System.Collections;

public class Smasher : MonoBehaviour {


    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
    }

    //test
    public float pushPower = 10f;


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Does the thing we hit have a rigid body?
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic) {

            // push it
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.velocity = pushDir * pushPower;



        }

    }

    void OnTriggerEnter(Collider other) {
        //Debug.Log("HITTING something with tag " + other.tag);
        // smashable things are now handled in the other object
        Explod explod = (Explod)other.GetComponent(typeof(Explod));
        if (explod != null) {
        //if (hit.gameObject.CompareTag("Breakable")) {
            // Check if fast enough (todo)
            explod.explod(); // FOR THE GREATER GOOD!
        }
    }
}
