#pragma warning disable 0414
/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;
using Game;

/**
 * \brief name-space for classes and method related to main scene
 */
namespace MainScene {
	public class SelectBrainPart : MonoBehaviour {
		public GameObject icon;
		public BrainPartName brainPartToLoad;
		public bool showOnAndroid = true;
		public bool CanSelect{ get; set; }

		private bool initialMouseOver;
		private Color selectionColor;
		private Color originalColor;
		private GameObject Icon { get; set; }

		void Start()
		{
			CanSelect = false;
			icon.renderer.material.color = new Color(icon.renderer.material.color.r, icon.renderer.material.color.g, icon.renderer.material.color.b, 0);
			icon.transform.position = this.transform.parent.transform.position;
			originalColor = this.renderer.material.color;
			initialMouseOver = true;
		}

#if UNITY_STANDALONE
		void OnMouseEnter()
		{
			if(CanSelect)
			{
				StartCoroutine("FadeIn");

                //highlight material
                this.renderer.material.color = new Color(originalColor.r + 0.4f, originalColor.g + 0.4f, originalColor.b + 0.4f);
				initialMouseOver = false;
			}
		}

		void OnMouseExit()
		{
			if(CanSelect)
				StartCoroutine("FadeOut");

			this.renderer.material.color = originalColor;
		}
#endif
		void OnMouseOver()
		{
			if(CanSelect)
			{
				if(Input.GetButtonDown ("Fire1"))
				{
					//highlight material
					this.renderer.material.color = new Color(originalColor.r + 0.4f, originalColor.g + 0.4f, originalColor.b + 0.4f);

                    print("Going into brain part: '" + brainPartToLoad + "'");
                    MGC mgc = MGC.Instance;
					
                    mgc.currentBrainPart = brainPartToLoad;
                    mgc.fromMain = true;
                    mgc.fromSelection = false;
					
                    
                    StopAllCoroutines();

                    mgc.sceneLoader.LoadScene("GameSelection");
				}
			}
			if(CanSelect && initialMouseOver)
			{
#if UNITY_STANDALONE
				OnMouseEnter();
#endif
				print ("Mouse over from initial animation.");
			}
		}

#if UNITY_ANDROID
		public void ShowIcon()
		{
			if(showOnAndroid)
			{
				StartCoroutine("FadeIn");
				initialMouseOver = false;
			}
		}
#endif

		IEnumerator FadeIn()
		{
			float startTime = Time.time;
			StopCoroutine ("FadeOut");
			Color startColor = icon.renderer.material.color;
			Color targetColor = icon.renderer.material.color;
			targetColor.a = 1;
			
			while(icon.renderer.material.color.a < 0.99f)
			{
				icon.renderer.material.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}
		}
		
		IEnumerator FadeOut()
		{
			float startTime = Time.time;
			StopCoroutine ("FadeIn");
			Color startColor = icon.renderer.material.color;
			Color targetColor = icon.renderer.material.color;
			targetColor.a = 0;
			
			while(icon.renderer.material.color.a > 0.01f)
			{
				icon.renderer.material.color = Color.Lerp (startColor, targetColor, Time.time - startTime);
				yield return null;
			}

			icon.renderer.material.color = targetColor;
		}
	}
}