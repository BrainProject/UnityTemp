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
//		public Texture helpTexture;
//		public bool helpExists;
		public GameObject pictureInHands;
		public GameObject questionMark;
		public GameObject confetti;
		public NEWBrainHelp newHelp;

		internal Animator animator;

//		private GameObject helpObject;

		void Start()
		{
			animator = this.GetComponent<Animator> ();
//			helpExists = false;
			MGC.Instance.neuronHelp = this.gameObject;
			MGC.Instance.ShowCustomCursor (true);
			
			questionMark.SetActive (newHelp.helpObject.helpPrefab != null);

			//Move Neuron according to screen aspect.
			//The solution is only for following aspect ratios (it might work with other ratios, but they are not supported anyway):
			//5:4, 4:3, 3:2, 16:10, 16:9
			Vector3 tmp;

			tmp = transform.parent.GetComponent<Camera>().ScreenToViewportPoint(new Vector3 (transform.parent.GetComponent<Camera>().WorldToScreenPoint(transform.localPosition).x, 0, 0));
			//tmp.x = tmp.x - transform.parent.camera.aspect / 2;
			tmp.x = 1.45f * transform.localPosition.x - (tmp.x - tmp.x/16)/Camera.main.aspect;
			tmp.y = transform.localPosition.y;
			tmp.z = transform.localPosition.z;
			
//			Debug.Log (tmp.x);
			transform.localPosition = tmp;
		}

//		void LateUpdate()
//		{
//			if(animator.GetBool("wave"))
//				animator.SetBool("wave", false);
//			if(animator.GetBool("smile"))
//				animator.SetBool("smile", false);
//		}

		//Attach GUI texture to make this function working
//		public void ShowHelpBubble (bool initialHelp = false) {
//			
//			if(initialHelp)
//			{
//				if(MGC.Instance.minigamesProperties.GetMinigamesWithHelp().Contains(Application.loadedLevelName))
//				{
//					MinigameProperties thisMinigame = MGC.Instance.minigamesProperties.GetMinigame(Application.loadedLevelName);
//					Debug.Log ("Help shown in this minigame " + thisMinigame.stats.initialShowHelpCounter + " times.");
//					++thisMinigame.stats.initialShowHelpCounter;
//					if(thisMinigame.stats.initialShowHelpCounter < 3)
//					{
//						if(helpTexture && !helpExists && MGC.Instance.minigamesProperties.GetMinigamesWithHelp().Contains(Application.loadedLevelName))
//						{
//							helpObject = (GameObject)Instantiate (Resources.Load ("Help"));
//							helpObject.guiTexture.texture = helpTexture;
//							//helpObject.transform.parent = this.transform;
//							//helpObject.layer = this.gameObject.layer;
//							helpObject.GetComponent<BrainHelpSettings>().neuronHelp = this.gameObject;
//						}
//						else
//						{
//							this.GetComponent<Animator>().SetBool("wave", true);
//						}
//					}
//				}
//			}
//			else
//			{
//				if(helpTexture && !helpExists && MGC.Instance.minigamesProperties.GetMinigamesWithHelp().Contains(Application.loadedLevelName))
//				{
//					helpObject = (GameObject)Instantiate ((Resources.Load ("Help")));
//					helpObject.guiTexture.texture = helpTexture;
//					//helpObject.transform.parent = this.transform;
//					//helpObject.layer = this.gameObject.layer;
//					helpObject.GetComponent<BrainHelpSettings>().neuronHelp = this.gameObject;
//					MGC.Instance.minigamesProperties.SetPlayed(Application.loadedLevelName);
//				}
//				else
//				{
//				//print ("here " + helpExists + ", '" + MGC.Instance.minigamesProperties.GetMinigamesWithHelp().Contains(Application.loadedLevelName) + "', "
//					//       + Application.loadedLevelName + ", ");
//					this.GetComponent<Animator>().SetBool("wave", true);
//				}
//			}
//		}

		public void LaunchConfetties ()
		{
			if(confetti)
			{
				Instantiate(confetti);
			}
		}

//		void OnMouseDown()
//		{
//			ShowHelpBubble ();
//		}

		void OnLevelWasLoaded(int level)
		{
			if(MGC.Instance.getSelectedMinigameProperties() && MGC.Instance.getSelectedMinigameProperties().mainScene == Application.loadedLevelName)
			{
				newHelp.helpObject.helpPrefab = MGC.Instance.getSelectedMinigameProperties ().helpPrefab;
				questionMark.SetActive (MGC.Instance.getSelectedMinigameProperties().helpPrefab);
			}
			else
			{
				if(newHelp.helpObject.helpClone)
				{
					Destroy(newHelp.helpObject.helpClone);
					Color tmp = MGC.Instance.minigamesGUI.hideHelpIcon.thisImage.color;
					tmp.a = 0;
					MGC.Instance.minigamesGUI.hideHelpIcon.thisImage.color = tmp;
					MGC.Instance.minigamesGUI.hideHelpIcon.gameObject.SetActive(false);
					tmp = MGC.Instance.minigamesGUI.replayHelpIcon.thisImage.color;
					tmp.a = 0;
					MGC.Instance.minigamesGUI.replayHelpIcon.thisImage.color = tmp;
					MGC.Instance.minigamesGUI.replayHelpIcon.gameObject.SetActive(false);
					newHelp.helpObject.StopShowingButtons();
					newHelp.helpObject.thisAnimator.SetTrigger("HideHelp");
				}

				newHelp.helpObject.helpPrefab = null;
				questionMark.SetActive (false);
			}




//			helpExists = false;
//			if(helpObject)
//			{
//				Destroy(helpObject);
//			}
//			
//			if(level == 2)
//			{
//				helpTexture = (Texture)(Resources.Load("Textures/HelpText"));
//				ShowHelpBubble(true);
//			}
//			else if(level > 2)
//			{
//				//if(!MGC.Instance.minigamesProperties.GetPlayed(Application.loadedLevelName))
//				//{
//					ShowHelpBubble(true);
//				//}
//			}
//			else
//			{
//				helpExists = false;
//				helpTexture = null;
//			}
		}

		public void ShowSmile(Texture smileTexture)
		{
			animator.SetTrigger ("smile");
			GetComponent<Collider>().enabled = false;
			pictureInHands.GetComponent<Renderer>().material.mainTexture = smileTexture;
		}

		void ActivateCollider()
		{
			GetComponent<Collider>().enabled = true;
		}
	}
}