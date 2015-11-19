#pragma warning disable 0414
/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;
using Game;
using UnityEngine.EventSystems;

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
			icon.GetComponent<Renderer>().material.color = new Color(icon.GetComponent<Renderer>().material.color.r, icon.GetComponent<Renderer>().material.color.g, icon.GetComponent<Renderer>().material.color.b, 0);
			icon.transform.position = this.transform.parent.transform.position;
			originalColor = this.GetComponent<Renderer>().material.color;
			initialMouseOver = true;
		}

#if UNITY_STANDALONE
		void OnMouseEnter()
		{
			if(CanSelect
#if UNITY_ANDROID
&& showOnAndroid
#endif
			   )
			{
				StartCoroutine("FadeIn");

                //highlight material
                this.GetComponent<Renderer>().material.color = new Color(originalColor.r + 0.4f, originalColor.g + 0.4f, originalColor.b + 0.4f);
				initialMouseOver = false;
			}
		}

		void OnMouseExit()
		{
			if(CanSelect
#if UNITY_ANDROID
&& showOnAndroid
#endif
			   )
				StartCoroutine("FadeOut");

			this.GetComponent<Renderer>().material.color = originalColor;
		}
#endif
		void OnMouseOver()
		{
			if(CanSelect && !EventSystem.current.IsPointerOverGameObject()
#if UNITY_ANDROID
			   && showOnAndroid
#endif
			   )
			{
				if(Input.GetButtonDown ("Fire1"))
				{
					//highlight material
					this.GetComponent<Renderer>().material.color = new Color(originalColor.r + 0.4f, originalColor.g + 0.4f, originalColor.b + 0.4f);

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
			Color startColor = icon.GetComponent<Renderer>().material.color;
			Color targetColor = icon.GetComponent<Renderer>().material.color;
			targetColor.a = 1;
			
			while(icon.GetComponent<Renderer>().material.color.a < 1)
			{
				icon.GetComponent<Renderer>().material.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}
		}
		
		IEnumerator FadeOut()
		{
			float startTime = Time.time;
			StopCoroutine ("FadeIn");
			Color startColor = icon.GetComponent<Renderer>().material.color;
			Color targetColor = icon.GetComponent<Renderer>().material.color;
			targetColor.a = 0;
			
			while(icon.GetComponent<Renderer>().material.color.a > 0)
			{
				icon.GetComponent<Renderer>().material.color = Color.Lerp (startColor, targetColor, Time.time - startTime);
				yield return null;
			}

			icon.GetComponent<Renderer>().material.color = targetColor;
		}
	}
}