using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class BackGUI : MonoBehaviour {
		public Animator deskAnimator;
		public GameObject Images;
		public LevelManagerColoring thisLevelManager;

//		void Start()
//		{
////			this.guiTexture.pixelInset = new Rect (50, Screen.height - Screen.height/9*3, Screen.width / 16 * 2, Screen.height / 9 * 2);
//		}
//
//		void OnMouseEnter()
//		{
//			//print ("enter");
//			this.guiTexture.texture = hover;
//		}
//
//		void OnMouseExit()
//		{
//			this.guiTexture.texture = normal;
//		}
//
//		void OnMouseDown()
//		{
//			BackAction ();
//		}

		public void BackAction()
		{
			if((Time.time - thisLevelManager.timestamp > 2) && Images.activeSelf)
			{
				thisLevelManager.timestamp = Time.time;
				deskAnimator.SetBool("painting", false);
				deskAnimator.SetTrigger("animate");
				thisLevelManager.painting = false;
				MGC.Instance.minigamesGUI.screenshotIcon.hide();
				MGC.Instance.ShowCustomCursor(true);
			}
		}

		public void IconVisible(bool isVisible)
		{
//			if(isVisible)
//			{
//				StartCoroutine ("FadeInGUI");
//				//thisLevelManager.neuronMaterial = thisLevelManager.neuronOriginalMaterial;
//			}
//			else
//			{
//				StartCoroutine("FadeOutGUI");
#if UNITY_ANDROID
			thisLevelManager.brushColor.renderer.material.color = Color.white;
			thisLevelManager.neuronMaterial.color = thisLevelManager.neuronOriginalColor;
#endif
//			}
		}
//
//		IEnumerator FadeInGUI()
//		{
//			float startTime = Time.time;
//			StopCoroutine ("FadeOutGUI");
//			Color startColor = this.guiTexture.color;
//			Color targetColor = this.guiTexture.color;
//			targetColor.a = 0.5f;
//			
//			while(this.guiTexture.color.a < 0.49f)
//			{
//				this.guiTexture.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
//				yield return null;
//			}
//		}
//		
//		IEnumerator FadeOutGUI()
//		{
//			float startTime = Time.time;
//			StopCoroutine ("FadeInGUI");
//			Color startColor = this.guiTexture.color;
//			Color targetColor = this.guiTexture.color;
//			targetColor.a = 0;
//			
//			while(this.guiTexture.color.a > 0.01f)
//			{
//				this.guiTexture.color = Color.Lerp (startColor, targetColor, Time.time - startTime);
//				yield return null;
//			}
//			this.gameObject.SetActive (false);
//		}
	}
}