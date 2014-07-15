using UnityEngine;
using System.Collections;

public class BeginningFade : MonoBehaviour {
	private bool fade;
	private Color originalColor;
	private Color targetColor;

	void Start () {
		fade = true;
		originalColor = this.guiTexture.color;
		originalColor.a = 0.6f;
		targetColor = this.guiTexture.color;
		targetColor.a = 0;
		print (originalColor.a);
	}
	
	// Update is called once per frame
	void Update () {
		if(fade)
		{
			this.guiTexture.color = Color.Lerp (originalColor, targetColor, (Time.timeSinceLevelLoad)/3);
		}
	}
}
