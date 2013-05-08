using UnityEngine;
using System.Collections;

public class RageBar : MonoBehaviour {

    //the player's rage and health
    public RageAndHealth player;

	//the textures
	public GUITexture barOutlineTex;
	public Texture2D barTex;

	// Use this for initialization
	void Start () {

		positionRageBarOutline();
	}
	
	// Update is called once per frame
	void Update () {
	
		positionRageBar();
	}

	void OnGUI() {

		//get the needed values
		float textureWidth = barTex.width;
		float textureHeight = barTex.height;
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;

		//find the texture ratio
		float textureAspectRatio = textureWidth / textureHeight;

		//calculate the drawing box for the texture
		float height = (screenHeight / 2.0f);
		float width = height * textureAspectRatio;
		float xPos = screenWidth - width - screenWidth / 110.0f + 2;
		float yPos = screenHeight - height - screenHeight / 110.0f - 2;

		//calcuate how much of the texture to draw
		float drawHeight = height * (player.getRage() / player.MAX_RAGE);

		//set the position
		GUI.BeginGroup(new Rect(xPos, yPos, width, drawHeight));
		GUI.Label(new Rect(0, 0, width, height), barTex);
		GUI.EndGroup();
	}

	//position the rage bar
	void positionRageBar() {


	}

	//position the rage bar outlone
	void positionRageBarOutline() {

		//get the needed values
		float textureWidth = barOutlineTex.texture.width;
		float textureHeight = barOutlineTex.texture.height;
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;

		//find the texture ratio
		float textureAspectRatio = textureWidth / textureHeight;

		//calculate the drawing box for the texture
		float height = screenHeight / 2.0f;
		float width = height * textureAspectRatio;
		float xPos = (screenWidth / 2.0f) - (width + (screenWidth / 110.0f));
		float yPos = -(screenHeight / 2.0f) + screenHeight / 110.0f;

		//set the position
		barOutlineTex.pixelInset = new Rect(xPos, yPos,
			width, height);
	}
}
