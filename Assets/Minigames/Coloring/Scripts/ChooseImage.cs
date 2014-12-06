using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class ChooseImage : MonoBehaviour
	{
		public GameObject Image;
		public LevelManagerColoring thisLevelManager;

		private Animator deskAnimator;

		void Start()
		{
			deskAnimator = transform.parent.transform.parent.GetComponent<Animator> ();
		}

		void OnMouseDown(){

			if(Time.time - thisLevelManager.timestamp > 2)
			{
				thisLevelManager.timestamp = Time.time;
				Image.SetActive(true);
				Image.transform.parent.gameObject.SetActive(true);
				deskAnimator.SetBool("painting", true);
				deskAnimator.SetTrigger("animate");
				thisLevelManager.painting = true;
				thisLevelManager.ShowColoringGUI(true);
				if(!thisLevelManager.hiddenGUIwhilePainting)
					MGC.Instance.ShowCustomCursor(false);
				MGC.Instance.minigameStates.SetPlayed(Application.loadedLevelName);
			}
		}

		void LateUpdate()
		{
			deskAnimator.SetBool("painting", false);
		}
	}
}