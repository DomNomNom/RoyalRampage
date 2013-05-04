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

	//the music that is currently playing
	private AudioSource currentlyPlaying;

	//the section time for the music
	private readonly float sectionTime = 5.992f;

	//the player's rage and health
	public RageAndHealth player;

	//FUNCTIONS
	// Use this for initialization
	void Start () {
	
		//start playing the music and check again in 6 seconds
		checkMusic();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//checks
	void checkMusic() {

		Debug.Log(player.rage);

		//get the player's rage level and check what music stage we should play
		if (player.rage < 17) {

			musicStage1.Play();
		}
		else if (player.rage < 34) {

			musicStage2.Play();
		}
		else if (player.rage < 51) {

			musicStage3.Play();
		}
		else if (player.rage < 68) {

			musicStage4.Play();
		}
		else if (player.rage < 85) {

			musicStage5.Play();
		}
		else {

			musicStage6.Play();
		}

		//check again in another 6 seconds
		Invoke("checkMusic", sectionTime);
	}
}
