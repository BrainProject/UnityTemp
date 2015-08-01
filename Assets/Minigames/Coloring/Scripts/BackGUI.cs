/**
 *@author Tomáš Pouzar
 */
using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class BackGUI : MonoBehaviour {
		public Texture normal;
		public Texture hover;
		
		public Animator deskAnimator;
		public GameObject Images;
		public LevelManagerColoring thisLevelManager;

		void Start()
		{                                         // 50
			this.guiTexture.pixelInset = new Rect (15, Screen.height - Screen.height/9*3, Screen.width / 16 * 2, Screen.height / 9 * 2);
		}

		void OnMouseEnter()
		{
			//print ("enter");
			this.guiTexture.texture = hover;
		}

		void OnMouseExit()
		{
			this.guiTexture.texture = normal;
		}

		void OnMouseDown()
		{
            if (!thisLevelManager.mixing)
            {
                if ((Time.time - thisLevelManager.timestamp > 2) && Images.activeSelf)
                {
                    thisLevelManager.timestamp = Time.time;
                    deskAnimator.SetBool("painting", false);
                    deskAnimator.SetTrigger("animate");
                    //				thisLevelManager.GetComponent<PaintingModeTransition>().SetImages(false);
                    //				thisLevelManager.GetComponent<PaintingModeTransition>().SetPalette(false);
                    thisLevelManager.painting = false;
                    thisLevelManager.ShowColoringGUI(false);
                    MGC.Instance.ShowCustomCursor(true);
                }
            }
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
				yield return null;
			}
			this.gameObject.SetActive (false);
		}
	}
}