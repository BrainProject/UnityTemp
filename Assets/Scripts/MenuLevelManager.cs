using UnityEngine;
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

		void Awake()
		{
			Instance = this;
		}

		void Start()
		{
			Debug.Log ("Creating " + MGC.Instance);
			MGC.Instance.minigamesGUI.backIcon.gameObject.SetActive (false);
			Color tmp = MGC.Instance.minigamesGUI.backIcon.thisImage.color;
			tmp.a = 0;
			MGC.Instance.minigamesGUI.backIcon.thisImage.color = tmp;
		}

		public void SwitchMenu(int sectionIndex)
		{
			previouslySelectedSectionIndex = currentlySelectedSectionIndex;
			currentlySelectedSectionIndex = sectionIndex;
			StartCoroutine (Fade ());

			//TODO: show back button if brain is not active
		}

		IEnumerator Fade()
		{
			blockPanel.SetActive (true);
			float startTime = Time.time;

			while(menuSections[previouslySelectedSectionIndex].alpha > 0.001)
			{
				menuSections[previouslySelectedSectionIndex].alpha = Mathf.Lerp (1, 0, (Time.time - startTime));
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
				menuSections[currentlySelectedSectionIndex].alpha = Mathf.Lerp (0, 1, (Time.time - startTime));
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
				kinectRequiredIcon.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}
			
			yield return new WaitForSeconds (1);
			
			startTime = Time.time;
			startColor = kinectRequiredIcon.color;
			targetColor = kinectRequiredIcon.color;
			targetColor.a = 0;
			
			while(kinectRequiredIcon.color.a > 0)
			{
				kinectRequiredIcon.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}
			
			kinectRequiredIcon.color = targetColor;
		}
	}
}
