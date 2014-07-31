/*
 * Created by: Milan Dolezal
 */

//TODO: Fade when level is selected

using UnityEngine;
using System.Collections;

namespace Game {
	public class LoadLevelWithFade: MonoBehaviour {
		public bool forBeginning { get; set; }
		private Color originalColor;
		private Color targetColor;
		private float startTime;
		private string levelName;

		void Awake()
		{
			this.guiTexture.pixelInset = new Rect(Screen.width/2, Screen.height/2, 0, 0);//Screen.width, Screen.height);
		}

		void Start()
		{
			//default value
			forBeginning = true;
			originalColor = this.guiTexture.color;
			targetColor = this.guiTexture.color;
			startTime = Time.time;
			StartCoroutine (LoadSeledctedLevelWithColorLerp (forBeginning));

	        //find Logger instance

		}
		
		// Update is called once per frame
	//	void Update () {
	//		if(forBeginning)
	//			this.guiTexture.color = Color.Lerp (originalColor, targetColor, (Time.timeSinceLevelLoad)/3);
	//		else
	//			this.guiTexture.color = Color.Lerp (originalColor, targetColor, (Time.time - startTime)/3);
	//
	//		if(this.guiTexture.color.a < 0.01f)
	//			Destroy(this.gameObject);
	//		if(this.guiTexture.color.a > 0.6f)
	//			Application.LoadLevel(levelName);
	//	}

		public IEnumerator LoadSeledctedLevelWithColorLerp(bool forBeginning, string levelToLoad = "")
		{
			if(forBeginning)
			{
				originalColor.a = 0.6f;
				targetColor.a = 0;
				while(this.guiTexture.color.a > 0.01f)
				{
					this.guiTexture.color = Color.Lerp (originalColor, targetColor,(Time.timeSinceLevelLoad)/3);
					
					yield return null;
				}
				this.gameObject.guiTexture.enabled = false;
			}
			else if(levelToLoad == "")
			{
				print ("No minigame here.");
				this.gameObject.guiTexture.enabled = false;
			}
			else
			{
				GameObject.Find ("KinectControls").SetActive(false);
				float startTime = Time.time;
				originalColor.a = 0;
				targetColor.a = 1.0f;
				this.gameObject.guiTexture.enabled = true;
				while(this.guiTexture.color.a < 0.6f)
				{
					this.guiTexture.color = Color.Lerp (originalColor, targetColor,(Time.time - startTime)/3);
					
					yield return null;
				}

				Application.LoadLevel(levelToLoad);
	            Logger.addLogEntry("Loading game scene: '" + levelToLoad + "'");
			}
		}
	}
}