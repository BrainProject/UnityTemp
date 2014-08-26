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
		internal Texture helpTexture;
		public bool helpExists;

		private GameObject helpObject;
		private Animator animator;

		void Start()
		{
			animator = this.GetComponent<Animator> ();
			helpExists = false;
		}

		void LateUpdate()
		{
			if(animator.GetBool("noAnimation"))
				animator.SetBool("noAnimation", false);
		}

		//Attach GUI texture
		void ShowHelpBubble () {
			if(helpTexture && !helpExists)
			{
				helpObject = (GameObject)Instantiate ((Resources.Load ("Help/Help")));
				helpObject.guiTexture.texture = helpTexture;
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
			if(level > 2)
				ShowHelpBubble();
		}
	}
}