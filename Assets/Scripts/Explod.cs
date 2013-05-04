using UnityEngine;
using System.Collections;

// This class name must be spoken with a russian accent.
public class Explod : MonoBehaviour {
    public GameObject[] debrisObjects;
    public GameObject smokeParticles;
    public int numDebris = 5;

    public float explosionPower = 10f;
    public float explosionRadius = 5f;
    public float explosionUpwardsModifier = 3f;

    //the amount of rage the player gets from this
    public readonly float RAGE_INCREASE = 4.0f;

    public void explod() {
        GameObject gameObject = transform.parent.gameObject;

        Instantiate (smokeParticles, gameObject.transform.position, Quaternion.identity);

        // Create debris
        for (int i=0; i<numDebris; i++) {
            GameObject debris = (GameObject)Instantiate(debrisObjects[Random.Range(0,debrisObjects.Length)], gameObject.transform.position, Random.rotation);
            debris.rigidbody.AddExplosionForce(explosionPower, gameObject.transform.position, explosionRadius, explosionUpwardsModifier);
        }

        //increase the player's rage
        ((RageAndHealth) GameObject.Find("Player").
            GetComponent(typeof(RageAndHealth))).increaseRage(RAGE_INCREASE);

        Destroy(gameObject);
    }

}
