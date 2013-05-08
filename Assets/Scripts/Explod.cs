using UnityEngine;
using System.Collections;

// This class name must be spoken with a russian accent.
public class Explod : MonoBehaviour {
    public GameObject[] debrisObjects;
    public GameObject smokeParticles;
	public float smokeLife = 5.0f;
    public int numDebris = 20;
	public float debrisLife = 15.0f;
	public float debrisScale = 1.5f;
	
    public float explosionPower = 200f;
    //public float explosionRadius = 5f;
    //public float explosionUpwardsModifier = 3f;

    private Transform world;

    //the amount of rage the player gets from this
    public readonly float RAGE_INCREASE = 2.0f;

    void Start() {		
        //world = smokeParticles.transform.parent.parent.Find("World"); // hopefully this will point to the TileManagers world
		world = GameObject.FindGameObjectWithTag("World").transform;
    }

    public void explod() {
		// Get the parent object, ie. the one that's getting smashed
        GameObject smashedObject = transform.parent.gameObject;

        

		// Create the smoke.
        GameObject smoke = (GameObject) Instantiate(smokeParticles, smashedObject.transform.position, Quaternion.identity);
        smoke.transform.parent = world; // we attach it to the World
		Destroy (smoke, smokeLife); // destroy smoke after a while

        // Create appropriate number of debris objects and make them explode out.
        for (int i=0; i<numDebris; i++) {

			// Select a random point on the surface of the smashed object to create the debris			
			Vector3 debrisPos = GetPointOnMesh(smashedObject).point;	
            GameObject debris = (GameObject)Instantiate(debrisObjects[Random.Range(0,debrisObjects.Length)],
				debrisPos, Random.rotation);
			
			// Scale if necessary
			if (debrisScale != 1.0f)
				debris.transform.localScale += new Vector3(debrisScale,debrisScale,debrisScale);
			
			// Attach the debris to the world object to prevent project manager spam
			debris.transform.parent = world;
			
			// Make the debris explode out away from the centre of the smashed object.
            //debris.rigidbody.AddExplosionForce(explosionPower, smashedObject.transform.position, explosionRadius, explosionUpwardsModifier);     
			debris.rigidbody.AddForce((debrisPos-smashedObject.transform.position).normalized 
				* Random.Range (explosionPower*0.8f, explosionPower+(explosionPower*0.2f)));
			
			// Debris expires after a time
			Destroy (debris, debrisLife);
        }

        //increase the player's rage
        ((RageAndHealth) GameObject.Find("Player").
            GetComponent(typeof(RageAndHealth))).increaseRage(RAGE_INCREASE);

        Destroy(smashedObject);
    }
	
	// Gets a random point on an object's mesh (assumes it has a collider)
	// this would be a good addition to a library
	public RaycastHit GetPointOnMesh(GameObject gameObject) {
	    float length = 100f;
	    Vector3 direction = Random.onUnitSphere;
	    Ray ray = new Ray(gameObject.transform.position + direction*length,-direction);
	    RaycastHit hit = new RaycastHit();
	    gameObject.collider.Raycast (ray, out hit, length*2);
	    return hit;
	}
}
