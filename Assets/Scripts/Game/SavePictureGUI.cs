using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Game
{
	public class SavePictureGUI : MonoBehaviour {
		public Texture normal;
		public Texture hover;
		public Image iconSave;
		public Image iconCheck;

		void Start()
		{
//			this.guiTexture.pixelInset = new Rect (50, Screen.height - Screen.height/9*5, Screen.width / 16 * 2, Screen.height / 9 * 2);
			//iconCheck.pixelInset = new Rect (50, Screen.height - Screen.height/9*5, Screen.width / 16 * 2, Screen.height / 9 * 2);
		}
		
//		void OnMouseEnter()
//		{
//			this.guiTexture.texture = hover;
//		}
//		
//		void OnMouseExit()
//		{
//			this.guiTexture.texture = normal;
//		}
		
		public void TakeScreenshot()
		{
			string dateText =/* "YYYY-MM-DD";*/ String.Format ("{0:yyyy-MM-dd--HH-mm-ss}", DateTime.Now);
			GameObject screenshotCamera = (GameObject)Resources.Load("ScreenshotCamera") as GameObject;
			screenshotCamera.GetComponent<RenderCameraToFile> ().RenderToFile ("Picture-" + dateText + ".png");
			StartCoroutine (GreenCheck());
			//MGC.Instance.logger.addEntry ("Snapshot saved into " + Application.persistentDataPath);
		}

		public void IconVisible(bool isVisible)
		{
			if(isVisible)
				StartCoroutine ("FadeInGUI");
			else
				StartCoroutine("FadeOutGUI");
		}
		
		IEnumerator FadeInGUI()
		{
			float startTime = Time.time;
			StopCoroutine ("FadeOutGUI");
			Color startColor = iconSave.color;
			Color targetColor = iconSave.color;
			targetColor.a = 1;
			
			while(iconSave.color.a < 1)
			{
				iconSave.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}
			//Time.timeScale = 0;
		}
		
		IEnumerator FadeOutGUI()
		{
			float startTime = Time.time;
			StopCoroutine ("FadeInGUI");
			Color startColor = iconSave.color;
			Color targetColor = iconSave.color;
			targetColor.a = 0;
			
			while(this.GetComponent<GUITexture>().color.a > 0)
			{
				iconSave.color = Color.Lerp (startColor, targetColor, Time.time - startTime);
				//Time.timeScale = state;
				yield return null;
			}
			this.gameObject.SetActive (false);
		}

		IEnumerator GreenCheck()
		{
			float startTime = Time.time;
			Color startColor = iconCheck.color;
			Color targetColor = iconCheck.color;
			targetColor.a = 1;
			iconCheck.enabled = true;
			
			while(iconCheck.color.a < 1)
			{
				float step = (Time.time - startTime) * 4;
				iconCheck.color = Color.Lerp (startColor, targetColor, step);
				yield return null;
			}

			yield return new WaitForSeconds (1);

			startTime = Time.time;
			startColor = iconCheck.color;
			targetColor = iconCheck.color;
			targetColor.a = 0;
			
			while(iconCheck.color.a > 0)
			{
				float step = (Time.time - startTime) / 2;
				iconCheck.color = Color.Lerp (startColor, targetColor, step);
				yield return null;
			}
			iconCheck.enabled = false;
		}
	}
}