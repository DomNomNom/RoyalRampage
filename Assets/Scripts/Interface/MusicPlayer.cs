using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	//VARIABLES
	//the different music stages
	public AudioSource musicStage1;
	public AudioSource musicStage2;
	public AudioSource musicStage3;
	public AudioSource musicStage4;
	public AudioSource musicStage5;
	public AudioSource musicStage6;

	//testing
	private float rage = 1.0f;

	//FUNCTIONS
	// Use this for initialization
	void Start () {
	
		//start playing the music and check again in 6 seconds
		musicStage1.Play();
		Invoke("checkMusic", 5.994f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void checkMusic() {

		if (rage < 100.0f) {

			rage = rage + 17.0f;
		}

		if (rage < 17) {

			musicStage1.Play();
		}
		else if (rage < 34) {

			musicStage2.Play();
		}
		else if (rage < 51) {

			musicStage3.Play();
		}
		else if (rage < 68) {

			musicStage4.Play();
		}
		else if (rage < 85) {

			musicStage5.Play();
		}
		else {

			musicStage6.Play();
		}



		//check again in another 6 seconds
		Invoke("checkMusic", 5.994f);
	}
}
