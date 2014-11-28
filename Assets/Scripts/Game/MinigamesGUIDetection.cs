using UnityEngine;
using System.Collections;

public class MinigamesGUIDetection : MonoBehaviour {
	public bool guiIsHidden = true;

	private Color startColor;
	private Color targetColor;
	//private Color startColorVisibleGUI;
	//private Color targetColorVisibleGUI;

	void Start ()
	{
		this.guiTexture.pixelInset = new Rect (Screen.width - Screen.width / 8, Screen.height / 9, Screen.width / 16, Screen.height / 9);

		startColor = this.guiTexture.color;
		targetColor = this.guiTexture.color;
		targetColor.a = 0.51f;
		//startColorVisibleGUI = targetColor;
		//targetColorVisibleGUI = Color.green;
	}

	void Update()
	{
//		if(Input.GetKeyDown(KeyCode.Keypad7))
		  	//ShowDetection();

		if(Input.GetKeyDown(KeyCode.Keypad9))
			HideDetection();

	}

	public void ShowDetection(float state)
	{
//		StartCoroutine ("FadeInGUI");
		if(guiIsHidden)
			this.guiTexture.color = Color.Lerp (startColor, targetColor, state);
		//else
		//	this.guiTexture.color = Color.Lerp (startColorVisibleGUI, targetColorVisibleGUI, state);
	}

	public void HideDetection()
	{
		StartCoroutine ("FadeOutGUI");
	}

//	IEnumerator FadeInGUI()
//	{
//		float startTime = Time.time;
//		StopCoroutine ("FadeOutGUI");
//		Color startColor = this.guiTexture.color;
//		Color targetColor = this.guiTexture.color;
//		targetColor.a = 1;
//		
//		while(this.guiTexture.color.a < 0.51f)
//		{
//			this.guiTexture.color = Color.Lerp (startColor, targetColor, (Time.time - startTime)/3);
//			yield return null;
//		}
//	}
	
	IEnumerator FadeOutGUI()
	{
		float startTime = Time.time;
		StopCoroutine ("FadeInGUI");
		Color startColor = this.guiTexture.color;
		Color targetColor = this.guiTexture.color;
		targetColor.a = 0;
		
		while(this.guiTexture.color.a > 0.01f)
		{
			this.guiTexture.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
			yield return null;
		}
		this.guiTexture.color = targetColor;
	}
}
