/**************************************************\
| Stores and controls the player's rage and health |
\**************************************************/

using UnityEngine;
using System.Collections;

public class RageAndHealth : MonoBehaviour {

    //VARIABLES
    //gui style
    public GUIStyle style = new GUIStyle();

    //is true if debug mode is enabled
    public bool debug = true;

    ///the maximum health the player can have
    private readonly int MAX_HEALTH = 100;
    //the maximum rage the player can have
    private readonly float MAX_RAGE = 100.0f;
    //the amount which the rage decreases
    private readonly float RAGE_DECREASE = 3.0f;

    //the values
    private int health = 100;
    private float rage = 15.0f;

    //timing
    private float startTime;

    //user needs to be spammed
    private bool spamalise = false;

    // Use this for initialization
    void Start () {
    
        startTime = Time.timeSinceLevelLoad;
    }
    
    // Update is called once per frame
    void Update () {
    
        //decrease rage over time
        float timeDiff = Time.timeSinceLevelLoad - startTime;
        deceaseRage(timeDiff * RAGE_DECREASE);
        startTime = Time.timeSinceLevelLoad;
    }

    /*void OnTriggerEnter(Collider other) {
        //Debug.Log("HITTING something with tag " + other.tag);
        // smashable things are now handled in the other object
        Explod explod = (Explod)other.GetComponent(typeof(Explod));
        if (explod != null) {

            increaseRage(4.0f);
        }
    }*/

    //show debug messages
    void OnGUI () {

        if (debug) {

            GUI.Label(new Rect(20, 80, 80, 20), "Rage: " + rage, style);
        }

        if (spamalise) {

            for (int i = 0; i < 30; ++i) {

                for (int j = 0; j < 10; ++j) {

                    float xShift = Random.Range(0, 10) / 2.0f;
                    float yShift = Random.Range(0, 10) / 2.0f;

                    GUI.Label(new Rect(0 + (160 * j) + xShift,
                        0 + (i * 20) + yShift, 100, 20),
                        "WHAT ARE YOU DOING?!", style);
                }
            }
        }
    }

    //decreases the rage
    public void deceaseRage(float amount) {

        //spam if someone passes a negative value (learning by punishment)
        if (amount < 0.0f) {

            spamalise = true;
        }

        rage -= amount;

        //make sure the rage doesn't go below zero
        if (rage < 0.0f) {

            rage = 0.0f;
        }
    }

    //increase the rage
    public void increaseRage(float amount) {

        //spam if someone passes a negative value (learning by punishment)
        if (amount < 0.0f) {

            spamalise = true;
        }

        rage += amount;

        //make sure the rage doesn't go above max rage
        if (rage > MAX_RAGE) {

            rage = MAX_RAGE;
        }
    }

    //decrease health
    public void deceaseHealth(int amount) {

        //spam if someone passes a negative value (learning by punishment)
        if (amount < 0) {

            spamalise = true;
        }

        health -= amount;

        //make sure the health doesn't go below zero
        if (health < 0) {

            health = 0;
        }
    }

    //increase the health
    public void increaseHealth(int amount) {

        //spam if someone passes a negative value (learning by punishment)
        if (amount < 0) {

            spamalise = true;
        }

        health += amount;

        //make sure the health doesn't go above max health
        if (health > MAX_HEALTH) {

            health = MAX_HEALTH;
        }
    }

    //return the rage
    public float getRage() {

        return rage;
    }

    //return the health
    public int getHealth() {

        return health;
    }
}
