using UnityEngine;
using System.Collections;
using System;

namespace Coloring
{
	public class SavePictureGUI : MonoBehaviour {
		public Texture normal;
		public Texture hover;

		void Start()
		{
			this.guiTexture.pixelInset = new Rect (50, Screen.height - Screen.height/9*5, Screen.width / 16 * 2, Screen.height / 9 * 2);
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
			string dateText = String.Format ("{0-yyyy-MM-dd}", DateTime.Now);
			Camera.main.GetComponent<RenderCameraToFile> ().RenderToFile ("snapshot-" + dateText);
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
	}
}