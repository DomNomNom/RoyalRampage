using UnityEngine;
using System.Collections;

// This class name must be spoken with a russian accent.
public class Explod : MonoBehaviour {
    
	/*
	 * Public field 'List of Debris':
	 * Allows for an exact pattern of debris to be created.
	 * First field, debris, is the game object to create
	 * Second field, count, is how many times to create it
	 */
	[System.Serializable]
	public class Debris
	{
		public GameObject debrisObject;
		public float scale = 1.0f;
		public int minCount = 1;
		public int maxCount = 1;
		
	}	
	public Debris[] listOfDebris;
	
	// Other public fields
    public GameObject smokeParticles;
	public float smokeLife = 5.0f;
    //public int numDebris = 20;
	public float debrisLife = 15.0f;
	public float debrisScale = 1.5f;
    public float explosionPower = 200f;
	public float rageIncrease = 2.0f;   //the amount of rage the player gets from smashing this
	public float debrisImmortalTime = 0.25f;
	
	// Private fields
	private float duration = 0.0f;
    private Transform world;
 
    void Start() {		
		world = GameObject.FindGameObjectWithTag("World").transform;
    }
	
	void Update() 
	{
		if (duration < debrisImmortalTime)
		{
			duration += Time.deltaTime;
		}
	}

    public void explod() {
		// Only smash if we've existed long enough
		if (duration < debrisImmortalTime)
		{
			return;	
		}
		
		// Get the parent object, ie. the one that's getting smashed
        GameObject smashedObject = transform.parent.gameObject;

		// Create the smoke.
        GameObject smoke = (GameObject) Instantiate(smokeParticles, smashedObject.transform.position, Quaternion.identity);
        smoke.transform.parent = world; // we attach it to the World
		Destroy (smoke, smokeLife); // destroy smoke after a while
		
		// Create debris.
		foreach (Debris d in listOfDebris) 
		{
			// Create appropriate number of debris objects and make them explode out.
			int count = Random.Range (d.minCount, d.maxCount+1);
	        for (int i=0; i<count; i++) {
	
				// Select a random point on the surface of the smashed object to create the debris			
				Vector3 debrisPos = GetPointOnMesh(smashedObject).point;	
	            GameObject debris = (GameObject)Instantiate(d.debrisObject, debrisPos, Random.rotation);
				
				// Scale 
				debris.transform.localScale += new Vector3(debrisScale*d.scale,debrisScale*d.scale,debrisScale*d.scale);
				
				// Attach the debris to the world object to prevent project manager spam
				debris.transform.parent = world;
				
				// Make the debris explode out away from the centre of the smashed object.  
				debris.rigidbody.AddForce((debrisPos-smashedObject.transform.position).normalized 
					* Random.Range (explosionPower*0.8f, explosionPower+(explosionPower*0.2f)));
				
				// Debris expires after a time
				Destroy (debris, debrisLife);
	        }
		}

        //increase the player's rage
        ((RageAndHealth) GameObject.Find("Player").
            GetComponent(typeof(RageAndHealth))).increaseRage(rageIncrease);

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

