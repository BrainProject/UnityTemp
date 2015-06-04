using UnityEngine;
using System.Collections;
using System;

namespace Coloring
{
	public class SavePictureGUI : MonoBehaviour {
		public Texture normal;
		public Texture hover;
		public GUITexture iconCheck;

		void Start()
		{
			this.guiTexture.pixelInset = new Rect (50, Screen.height - Screen.height/9*5, Screen.width / 16 * 2, Screen.height / 9 * 2);
			iconCheck.pixelInset = new Rect (50, Screen.height - Screen.height/9*5, Screen.width / 16 * 2, Screen.height / 9 * 2);
		}
		
		void OnMouseEnter()
		{
			this.guiTexture.texture = hover;
		}
		
		void OnMouseExit()
		{
			this.guiTexture.texture = normal;
		}
		
		void OnMouseDown()
		{
			string dateText =/* "YYYY-MM-DD";*/ String.Format ("{0:yyyy-MM-dd--HH-mm-ss}", DateTime.Now);
			Camera.main.GetComponent<RenderCameraToFile> ().RenderToFile ("Picture-" + dateText + ".png");
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
			Color startColor = this.guiTexture.color;
			Color targetColor = this.guiTexture.color;
			targetColor.a = 0.5f;
			
			while(this.guiTexture.color.a < 0.49f)
			{
				this.guiTexture.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}
			//Time.timeScale = 0;
		}
		
		IEnumerator FadeOutGUI()
		{
			float startTime = Time.time;
			StopCoroutine ("FadeInGUI");
			Color startColor = this.guiTexture.color;
			Color targetColor = this.guiTexture.color;
			targetColor.a = 0;
			
			while(this.guiTexture.color.a > 0.01f)
			{
				this.guiTexture.color = Color.Lerp (startColor, targetColor, Time.time - startTime);
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
			targetColor.a = 0.5f;
			iconCheck.enabled = true;
			
			while(iconCheck.color.a < 0.49f)
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