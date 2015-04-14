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


			//Move Neuron according to screen aspect.
			//The solution is only for following aspect ratios (it might work with other ratios, but they are not supported anyway):
			//5:4, 4:3, 3:2, 16:10, 16:9
			Vector3 tmp;

			tmp = transform.parent.camera.ScreenToViewportPoint(new Vector3 (transform.parent.camera.WorldToScreenPoint(transform.localPosition).x, 0, 0));
			//tmp.x = tmp.x - transform.parent.camera.aspect / 2;
			tmp.x = 1.45f * transform.localPosition.x - (tmp.x - tmp.x/16)/Camera.main.aspect;
			tmp.y = transform.localPosition.y;
			tmp.z = transform.localPosition.z;
			
			Debug.Log (tmp.x);
			transform.localPosition = tmp;
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
			
			if(initialHelp)
			{
				if(MGC.Instance.minigamesProperties.GetMinigamesWithHelp().Contains(Application.loadedLevelName))
				{
					MinigameProperties thisMinigame = MGC.Instance.minigamesProperties.GetMinigame(Application.loadedLevelName);
					Debug.Log ("Help shown in this minigame " + thisMinigame.stats.initialShowHelpCounter + " times.");
					++thisMinigame.stats.initialShowHelpCounter;
					if(thisMinigame.stats.initialShowHelpCounter < 3)
					{
						if(helpTexture && !helpExists && MGC.Instance.minigamesProperties.GetMinigamesWithHelp().Contains(Application.loadedLevelName))
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
			}
			else
			{
				if(helpTexture && !helpExists && MGC.Instance.minigamesProperties.GetMinigamesWithHelp().Contains(Application.loadedLevelName))
				{
					helpObject = (GameObject)Instantiate ((Resources.Load ("Help")));
					helpObject.guiTexture.texture = helpTexture;
					//helpObject.transform.parent = this.transform;
					//helpObject.layer = this.gameObject.layer;
					helpObject.GetComponent<BrainHelpSettings>().neuronHelp = this.gameObject;
					MGC.Instance.minigamesProperties.SetPlayed(Application.loadedLevelName);
				}
				else
				{
				//print ("here " + helpExists + ", '" + MGC.Instance.minigamesProperties.GetMinigamesWithHelp().Contains(Application.loadedLevelName) + "', "
					//       + Application.loadedLevelName + ", ");
					this.GetComponent<Animator>().SetBool("wave", true);
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
				//if(!MGC.Instance.minigamesProperties.GetPlayed(Application.loadedLevelName))
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