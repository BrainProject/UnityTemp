/*
 * Created by: Milan Dolezal
 */

using UnityEngine;
using System.Collections;

namespace Game {
	public class SplashScreen: MonoBehaviour {
		private Color originalColor;
		private Color targetColor;

		void Awake()
		{
			this.guiTexture.pixelInset = new Rect(Screen.width/2, Screen.height/2, 1, 1);//Screen.width, Screen.height);
		}

		void Start()
		{
			originalColor = this.guiTexture.color;
			targetColor = this.guiTexture.color;
			Screen.showCursor = false;
			StopCoroutine ("LoadLevelWithFade");
			StartCoroutine (LoadSeledctedLevelWithColorLerp ());
		}

		void Update()
		{
			if(Input.GetMouseButtonDown(0))
			{
				Screen.showCursor = true;
				Application.LoadLevel("Main");
			}
		}

		public IEnumerator LoadSeledctedLevelWithColorLerp()
		{
			originalColor.a = 0;
			targetColor.a = 1.0f;
			while(this.guiTexture.color.a < 0.99f)
			{
				this.guiTexture.color = Color.Lerp (originalColor, targetColor,(Time.timeSinceLevelLoad)/2);

				yield return null;
			}
			float startTime = Time.time + 1;
			originalColor.a = 0.99f;
			targetColor.a = 0;
			while(this.guiTexture.color.a > 0.01f)
			{
				this.guiTexture.color = Color.Lerp (originalColor, targetColor,(Time.time - startTime)/2);

				yield return null;
			}
			this.gameObject.guiTexture.enabled = false;
			Screen.showCursor = true;
			Application.LoadLevel((Application.loadedLevel)+1);
		}
	}
}