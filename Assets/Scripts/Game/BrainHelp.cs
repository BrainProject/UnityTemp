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

		internal Animator animator;

		private GameObject helpObject;

		void Start()
		{
			animator = this.GetComponent<Animator> ();
			helpExists = false;
			MGC.Instance.neuronHelp = this.gameObject;
			MGC.Instance.ShowCustomCursor ();
		}

		void LateUpdate()
		{
			if(animator.GetBool("wave"))
				animator.SetBool("wave", false);
			if(animator.GetBool("smile"))
				animator.SetBool("smile", false);
		}

		//Attach GUI texture to make this function working
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
				this.GetComponent<Animator>().SetBool("wave", true);
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

		public void ShowSmile(Texture smileTexture)
		{
			animator.SetBool ("smile", true);
			pictureInHands.renderer.material.mainTexture = smileTexture;
		}
	}
}