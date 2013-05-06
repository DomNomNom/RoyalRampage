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

    private Transform world;

    //the amount of rage the player gets from this
    public readonly float RAGE_INCREASE = 4.0f;

    void Start() {
        world = smokeParticles.transform.parent.parent.Find("World"); // hopefully this will point to the TileManagers world
    }

    public void explod() {
        GameObject gameObject = transform.parent.gameObject;


        GameObject smoke = (GameObject) Instantiate(smokeParticles, gameObject.transform.position, Quaternion.identity);
        smoke.transform.parent = world; // we attach it to the World

        // Create debris
        for (int i=0; i<numDebris; i++) {
            GameObject origial = debrisObjects[Random.Range(0,debrisObjects.Length)];
            GameObject debris = (GameObject)Instantiate(origial, gameObject.transform.position, Random.rotation);
            debris.rigidbody.AddExplosionForce(explosionPower, gameObject.transform.position, explosionRadius, explosionUpwardsModifier);
            debris.transform.parent = world; // we attach it to the World
        }

        //increase the player's rage
        ((RageAndHealth) GameObject.Find("Player").
            GetComponent(typeof(RageAndHealth))).increaseRage(RAGE_INCREASE);

        Destroy(gameObject);
    }

}
