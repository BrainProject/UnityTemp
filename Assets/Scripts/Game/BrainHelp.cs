using UnityEngine;
using System.Collections;

/// <summary>
/// Help handles pop-up bubbles showing how to play currently started minigame.
/// Attach this scrip to object that lives across scenes (e.g.: Neuron)
/// \author: Milan Doležal
/// </summary>

namespace Game
{
	public class BrainHelp : MonoBehaviour {
		public Texture helpTexture;
		public bool helpExists;
		public GameObject pictureInHands;

		private GameObject helpObject;
		private Animator animator;

		void Start()
		{
			animator = this.GetComponent<Animator> ();
			helpExists = false;
			MGC.Instance.neuronHelp = this.gameObject;
		}

		void LateUpdate()
		{
			if(animator.GetBool("noAnimation"))
				animator.SetBool("noAnimation", false);
			if(animator.GetBool("sadSmile"))
				animator.SetBool("sadSmile", false);
		}

		//Attach GUI texture
		void ShowHelpBubble () {
			if(helpTexture && !helpExists)
			{
				helpObject = (GameObject)Instantiate ((Resources.Load ("Help")));
				helpObject.guiTexture.texture = helpTexture;
				//helpObject.transform.parent = this.transform;
				//helpObject.layer = this.gameObject.layer;
				helpObject.GetComponent<BrainHelpSettings>().neuronHelp = this.gameObject;
			}
			else
			{
				this.GetComponent<Animator>().SetBool("noAnimation", true);
			}
		}

		void OnMouseDown()
		{
			ShowHelpBubble ();
		}

		void OnLevelWasLoaded(int level)
		{
			if(helpObject)
				Destroy(helpObject);

			if(level > 2)
			{
				ShowHelpBubble();
			}
			else
			{
				helpExists = false;
				helpTexture = null;
			}
		}
	}
}