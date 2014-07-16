/*
 * Created by: Milan Dolezal
 */

//TODO: Fade when level is selected

using UnityEngine;
using System.Collections;

public class WhiteFade : MonoBehaviour {
	public bool forBeginning { get; set; }
	private Color originalColor;
	private Color targetColor;
	private float startTime;
	private string levelName;

	void Start () {
		//default value
		forBeginning = true;
		originalColor = this.guiTexture.color;
		targetColor = this.guiTexture.color;
		startTime = Time.time;
		if(forBeginning)
		{
			originalColor.a = 0.6f;
			targetColor.a = 0;
		}
		else
		{
			originalColor.a = 0;
			targetColor.a = 1.0f;
		}
//		print (originalColor.a);
	}
	
	// Update is called once per frame
	void Update () {
		if(forBeginning)
			this.guiTexture.color = Color.Lerp (originalColor, targetColor, (Time.timeSinceLevelLoad)/3);
		else
			this.guiTexture.color = Color.Lerp (originalColor, targetColor, (Time.time - startTime)/3);

		if(this.guiTexture.color.a < 0.01f)
			Destroy(this.gameObject);
		if(this.guiTexture.color.a > 0.6f)
			Application.LoadLevel(levelName);
	}
}
