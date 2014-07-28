using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {
	private GUITexture fadeImage;
	private float startTime;
	private string levelName;
	private bool fade;
	private Color originalColor;
	private Color targetColor;
	// Use this for initialization
	void Start () {
		fadeImage = this.transform.GetChild (0).guiTexture;
		fade = false;
		originalColor = fadeImage.guiTexture.color;
		targetColor = fadeImage.guiTexture.color;
		targetColor.a = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(fade)
		{
			//fadeImage.guiTexture.color = Color.Lerp (originalColor, targetColor, (Time.time - startTime)/3);
//			if(fadeImage.guiTexture.color.a >= 0.6f)
			{
				//Application.LoadLevel(levelName);
			}
		}
	}

	public void LoadSeledctedLevelWithColorLerp(string levelName, float startTime)
	{
		this.startTime = startTime;
		this.levelName = levelName;
		fade = true;
		//Application.LoadLevel (levelName);
	}


}
