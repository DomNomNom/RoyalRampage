using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    //VARIABLES
    //is true to enable music
    public bool playMusic = true
    ;

    //the different music stages
    public AudioSource musicStage1;
    public AudioSource musicStage2;
    public AudioSource musicStage3;
    public AudioSource musicStage4;
    public AudioSource musicStage5;
    public AudioSource musicStage6;

    //the player's rage and health
    public RageAndHealth player;

    //the standard volume for the music
    private readonly float MUSIC_VOLUME = 0.5f;
    //the fade rate
    private readonly float FADE_RATE = 0.1f;
    //the section time for the music
    private readonly float SECTION_TIME = 5.992f;

    //the music that is currently playing
    private AudioSource currentMusic;
    //the music that should being faded out
    private AudioSource fadeMusic;

    //is true if the last music should be faded out
    private bool fadeLast = false;
    //the current fade volume
    private float fade = 0.5f;

    //FUNCTIONS
    // Use this for initialization
    void Start () {

        if (playMusic) {

            //init variables
            currentMusic = musicStage1;
            fadeMusic = musicStage1;

            //start music playing
            currentMusic.Play();

            //start the check cycle
            checkMusic();
        }
    }
    
    // Update is called once per frame
    void Update () {
    
        if (fadeLast && fade > 0.0f) {

            fade -= FADE_RATE;
            fadeMusic.volume = fade;
        }
        else if (fadeLast) {

            fadeMusic.Stop();
        }
    }

    //checks if the music needs to change
    void checkMusic() {

        //get the player's rage level and check what music stage we should play
        if (player.getRage() < 17) {

            changeMusic(musicStage1);
        }
        else if (player.getRage() < 34) {

            changeMusic(musicStage2);
        }
        else if (player.getRage() < 51) {

            changeMusic(musicStage3);
        }
        else if (player.getRage() < 68) {

            changeMusic(musicStage4);
        }
        else if (player.getRage() < 85) {

            changeMusic(musicStage5);
        }
        else {

            changeMusic(musicStage6);
        }

        //check again at the end of the next section
        Invoke("checkMusic", SECTION_TIME);
    }

    //changes the music, pass in the new music stage
    void changeMusic(AudioSource newMusic) {

        //check if we actually need to change the music
        if (currentMusic != newMusic) {

            //fade out the currently playing music
            fadeMusic = currentMusic;
            currentMusic = newMusic;

            //make sure the music is at an acceptable volume and play
            currentMusic.volume = MUSIC_VOLUME;
            currentMusic.Play();

            //reset the fade volume
            fade = MUSIC_VOLUME;
            //set music to fade out
            fadeLast = true;
        }
    }
}
