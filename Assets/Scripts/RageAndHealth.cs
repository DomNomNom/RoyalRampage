/**************************************************\
| Stores and controls the player's rage and health |
\**************************************************/

using UnityEngine;
using System.Collections;

public class RageAndHealth : MonoBehaviour {

	//VARIABLES
	//the values
	public int health = 100;
	public int rage = 35;

	//the max values
	private int maxHealth = 100;
	private int maxRage = 100;

	//TESTING
	private int startTime;

	// Use this for initialization
	void Start () {
	
		startTime = (int) Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
	
		int currentTime = (int) Time.timeSinceLevelLoad;

		if (currentTime - startTime >= 3) {

			startTime = currentTime;
			rage += 5;
		}
	}
}
