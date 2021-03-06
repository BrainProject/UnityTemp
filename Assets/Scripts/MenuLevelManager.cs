﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MinigameSelection
{
	public class MenuLevelManager : MonoBehaviour
	{
		public static MenuLevelManager Instance { get; private set; }

		public List<CanvasGroup> menuSections = new List<CanvasGroup>();
		public Image kinectRequiredIcon;	//TODO: Redo to Unity UI
		public GameObject blockPanel;
		
		private int currentlySelectedSectionIndex;
		private int previouslySelectedSectionIndex;

        private float fadeSpeed;

        void Awake()
        {
            Instance = this;
			Debug.Log ("Creating " + MGC.Instance);
		}

        void Start()
        {
            if (MGC.Instance.minigamesGUI.backIcon.thisImage)
            {
                MGC.Instance.minigamesGUI.backIcon.gameObject.SetActive(false);
                Color tmp = MGC.Instance.minigamesGUI.backIcon.thisImage.color;
                tmp.a = 0;
                MGC.Instance.minigamesGUI.backIcon.thisImage.color = tmp;
            }
			blockPanel.SetActive (false);

            // Jump to section when returning from minigame.
            if (MGC.Instance.selectedMenuSectionIndex > 0)
            {
                MGC.Instance.minigamesGUI.backIcon.gameObject.SetActive(true);
                MGC.Instance.minigamesGUI.backIcon.show();
                menuSections[0].alpha = 0;
                menuSections[0].gameObject.SetActive(false);
                menuSections[MGC.Instance.selectedMenuSectionIndex].alpha = 1;
                menuSections[MGC.Instance.selectedMenuSectionIndex].gameObject.SetActive(true);
                previouslySelectedSectionIndex = 0;
                currentlySelectedSectionIndex = MGC.Instance.selectedMenuSectionIndex;
            }

            fadeSpeed = MGC.Instance.fadeSpeed;
        }

		public void SwitchMenu(int sectionIndex)
		{
            /*if (sectionIndex == 2) //DEV NOTE: Only for the empty painting section. When more minigames will be added, remove if block completely and use only else section!
            {
                MGC.Instance.sceneLoader.LoadScene("Coloring");
                MGC.Instance.minigamesGUI.backIcon.show();
            }
            else
            {*/
                previouslySelectedSectionIndex = currentlySelectedSectionIndex;
                currentlySelectedSectionIndex = sectionIndex;
                MGC.Instance.selectedMenuSectionIndex = sectionIndex;
                StartCoroutine(Fade());
            //}
		}

		IEnumerator Fade()
		{
			blockPanel.SetActive (true);
			float startTime = Time.time;

			while(menuSections[previouslySelectedSectionIndex].alpha > 0.001)
			{
				menuSections[previouslySelectedSectionIndex].alpha = Mathf.Lerp (1, 0, (Time.time - startTime) * fadeSpeed);
				yield return null;
			}
			menuSections [previouslySelectedSectionIndex].gameObject.SetActive (false);
			if(currentlySelectedSectionIndex > 0)
			{
				MGC.Instance.minigamesGUI.backIcon.gameObject.SetActive (true);
				MGC.Instance.minigamesGUI.backIcon.show();
			}

			menuSections [currentlySelectedSectionIndex].alpha = 0;
			menuSections [currentlySelectedSectionIndex].gameObject.SetActive (true);
			
			startTime = Time.time;

			while(menuSections[currentlySelectedSectionIndex].alpha < 1)
			{
				menuSections[currentlySelectedSectionIndex].alpha = Mathf.Lerp (0, 1, (Time.time - startTime) * fadeSpeed);
				yield return null;
			}
			blockPanel.SetActive (false);
		}


		
		public void FadeInOutKinectIcon()
		{
			StopCoroutine ("FadeInOutKinect");
			StartCoroutine ("FadeInOutKinect");
		}
		
		IEnumerator FadeInOutKinect()
		{
			float startTime = Time.time;
			Color startColor = kinectRequiredIcon.color;
			Color targetColor = kinectRequiredIcon.color;
			targetColor.a = 1;
			
			while(kinectRequiredIcon.color.a < 1)
			{
				kinectRequiredIcon.color = Color.Lerp (startColor, targetColor, (Time.time - startTime) * fadeSpeed);
				yield return null;
			}
			
			yield return new WaitForSeconds (1);
			
			startTime = Time.time;
			startColor = kinectRequiredIcon.color;
			targetColor = kinectRequiredIcon.color;
			targetColor.a = 0;
			
			while(kinectRequiredIcon.color.a > 0)
			{
				kinectRequiredIcon.color = Color.Lerp (startColor, targetColor, (Time.time - startTime) * fadeSpeed);
				yield return null;
			}
			
			kinectRequiredIcon.color = targetColor;
		}
	}
}
