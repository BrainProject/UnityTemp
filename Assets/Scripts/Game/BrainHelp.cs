using UnityEngine;
using System.Collections;


/**
 * \brief Various classes and methods available across all scenes 
 */
namespace Game
{
	
	/// <summary>
/// Help handles pop-up bubbles showing how to play currently started minigame.
/// Attach this scrip to Neuron character.
/// \author: Milan Doležal
/// </summary>
	public class BrainHelp : MonoBehaviour{
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
			MGC.Instance.ShowCustomCursor (true);
		}

		void LateUpdate()
		{
			if(animator.GetBool("wave"))
				animator.SetBool("wave", false);
			if(animator.GetBool("smile"))
				animator.SetBool("smile", false);
		}

		//Attach GUI texture to make this function working
		public void ShowHelpBubble (bool initialHelp = false) {
			if(MGC.Instance.minigameStates.GetMinigamesWithHelp().Contains(Application.loadedLevelName))
			{
				Minigame thisMinigame = MGC.Instance.minigameStates.GetMinigame(Application.loadedLevelName);
				if(initialHelp)
				{
					print (thisMinigame.initialShowHelpCounter);
					++thisMinigame.initialShowHelpCounter;
					if(thisMinigame.initialShowHelpCounter < 3)
					{
						if(helpTexture && !helpExists && MGC.Instance.minigameStates.GetMinigamesWithHelp().Contains(Application.loadedLevelName))
						{
							helpObject = (GameObject)Instantiate (Resources.Load ("Help"));
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
				}
				else
				{
					if(helpTexture && !helpExists && MGC.Instance.minigameStates.GetMinigamesWithHelp().Contains(Application.loadedLevelName))
					{
						helpObject = (GameObject)Instantiate ((Resources.Load ("Help")));
						helpObject.guiTexture.texture = helpTexture;
						//helpObject.transform.parent = this.transform;
						//helpObject.layer = this.gameObject.layer;
						helpObject.GetComponent<BrainHelpSettings>().neuronHelp = this.gameObject;
						MGC.Instance.minigameStates.SetPlayed(Application.loadedLevelName);
					}
					else
					{
						//print ("here " + helpExists + ", '" + MGC.Instance.minigameStates.GetMinigamesWithHelp().Contains(Application.loadedLevelName) + "', "
						//       + Application.loadedLevelName + ", ");
						this.GetComponent<Animator>().SetBool("wave", true);
					}
				}
			}
		}

		void OnMouseDown()
		{
			ShowHelpBubble ();
		}

		void OnLevelWasLoaded(int level)
		{
			helpExists = false;
			if(helpObject)
			{
				Destroy(helpObject);
			}
			
			if(level == 2)
			{
				helpTexture = (Texture)(Resources.Load("Textures/HelpText"));
				ShowHelpBubble(true);
			}
			else if(level > 2)
			{
				//if(!MGC.Instance.minigameStates.GetPlayed(Application.loadedLevelName))
				//{
					ShowHelpBubble(true);
				//}
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
			collider.enabled = false;
			pictureInHands.renderer.material.mainTexture = smileTexture;
		}

		void ActivateCollider()
		{
			collider.enabled = true;
		}
	}
}