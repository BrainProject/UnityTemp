using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game
{
	public class MinigamesGUIIconsActions : MonoBehaviour
    {
		public string action;
		public bool defaultDisabled = true;
		public bool useGSIIcons;	//TODO
		//public Texture2D texture_normal;
		//public Texture2D texture_hover;
		public Texture2D texture_normalGSI;
		public Texture2D texture_hoverGSI;
		
		internal Color startColor;
		internal Color targetColor;
		internal Image thisImage;
		internal Button thisButton;

		void Start()
		{
			thisImage = GetComponent<Image> ();
			thisButton = GetComponent<Button> ();
			startColor = thisImage.color;
			targetColor = thisImage.color;
			if (defaultDisabled)
			{
				thisButton.enabled = false;
				this.gameObject.SetActive (false);
			}
			else
			{
				this.gameObject.SetActive (true);
				thisButton.enabled = true;
			}
		}
        
//		public void resetState()
//        {
//			if(transform.parent.GetComponent<MinigamesGUI>().gsiStandalone)
//				renderer.material.mainTexture = texture_normalGSI;
//			else
//	            renderer.material.mainTexture = texture_normal;
//        }

//        void OnMouseEnter()
//		{
//			if(transform.parent.GetComponent<MinigamesGUI>().gsiStandalone)
//          		renderer.material.mainTexture = texture_hoverGSI;
//			else
//				renderer.material.mainTexture = texture_hover;
//        }

//        void OnMouseExit()
//        {
//            resetState();
//        }

        public void GUIAction()
        {
			MinigamesGUI parent = transform.parent.GetComponent<MinigamesGUI> ();
			parent.hide ();
			parent.clicked = true;

            //resolve action
            switch(action)
			{
				case "Restart":
	            {
	                //hide GUI
	                MGC.Instance.minigamesGUI.hide();

					if(SceneManager.GetActiveScene().buildIndex > 3)
	                    MGC.Instance.startMiniGame(MGC.Instance.getSelectedMinigameName());
					else
						MGC.Instance.sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
					break;
	            }

				case "GameSelection":
	            {
	                //hide GUI
	                MGC.Instance.minigamesGUI.hide();

	                //return to game selection scene
	                MGC.Instance.sceneLoader.LoadScene(2);

					break;
	            }

				case "Reward":
	            {
	                
	                //run external application with reward
	                //TODO solve things like controlling and closing external application and mainly - how to return to Unity

	                string path = @"-k http://musee.louvre.fr/visite-louvre/index.html?defaultView=rdc.s46.p01&lang=ENG";
	                Process foo = new Process();
	                foo.StartInfo.FileName = "iexplore.exe";
	                foo.StartInfo.Arguments = path;
	                foo.Start();

					break;
	            }
				
				case "Brain":
				{
                    //reset menu index
                    MGC.Instance.selectedMenuSectionIndex = 0;

					//hide GUI
					MGC.Instance.minigamesGUI.hide();
					
					//return to game selection scene
					MGC.Instance.sceneLoader.LoadScene("Crossroad");
					
					break;
				}

				case "Back":
				{
					//hide GUI
					MGC.Instance.minigamesGUI.hide();
					
					//return back
					if(SceneManager.GetActiveScene().buildIndex > 6)	//NOTE: Update minimal minigame level index here
					{
						// Coloring mini-game se special treatment...
						if (SceneManager.GetActiveScene().name == "Coloring")
						{
							Coloring.LevelManagerColoring coloringLM = GameObject.Find("_LevelManager").GetComponent<Coloring.LevelManagerColoring>();
							if (coloringLM.painting)
							{
								coloringLM.backGUI.BackAction();
							}
							else
							{
								MGC.Instance.sceneLoader.LoadScene("Crossroad");
							}
						}
						
						//back button in other mini-games
						else
						{
							int maxDiff = MGC.Instance.getSelectedMinigameProperties().MaxDifficulty;
							print("game has maxDiff: " + maxDiff);
							if(maxDiff == 0)
							{
								MGC.Instance.sceneLoader.LoadScene("Crossroad");
							}
							else
							{
								MGC.Instance.sceneLoader.LoadScene("DifficultyChooser");
							}
							
						}
					}
					
					else if(SceneManager.GetActiveScene().name == "DifficultyChooser")
					{
						MGC.Instance.sceneLoader.LoadScene("Crossroad");
					}
					else if(SceneManager.GetActiveScene().name == "GameSelection")
					{
						//Zoom out in selection scene if zoomed to some minigame.
						//Go to brain scene if not zoomed.
						MinigameSelection.CameraControl cm = Camera.main.GetComponent<MinigameSelection.CameraControl>();
						if(cm.ReadyToLeave)
						{
							MGC.Instance.fromMain = true;
							MGC.Instance.sceneLoader.LoadScene(1);
						}
						else
							cm.ZoomOutCamera();
					}
					else if(SceneManager.GetActiveScene().name == "TiledMenu")
					{
						hide();
						MinigameSelection.MenuLevelManager.Instance.SwitchMenu (0);
					}
					
					
					break;
				}

				case "Screenshot":
                {
                    GetComponent<SavePictureGUI>().TakeScreenshot();
                    show();
					break;
				}

				case "ReplayHelp":
				{
					MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject.ReplayHelpAnimation();
					this.hide();
					MGC.Instance.minigamesGUI.hideHelpIcon.hide();
					break;
				}

				case "HideHelp":
				{
					MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject.HideHelpAnimation();
					this.hide();
					MGC.Instance.minigamesGUI.replayHelpIcon.hide();
					break;
				}

				case "ShowHelp":
				{
					MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject.ShowHelpAnimation();
					MGC.Instance.minigamesGUI.hide();
					break;
				}
			}   
        }

		public void show()
		{
			this.gameObject.SetActive (true);
			StartCoroutine ("FadeInGUI");
		}

		public void hide()
		{
			this.gameObject.SetActive (true);
			StartCoroutine ("FadeOutGUI");
		}

		IEnumerator FadeInGUI()
		{
			float startTime = Time.time;
			thisButton.enabled = true;
			StopCoroutine ("FadeOutGUI");
	//		collider.enabled = true;
			startColor = thisImage.color;
			targetColor = thisImage.color;
			targetColor.a = 1;
			
			while(thisImage.color.a < 0.99f)
			{
				thisImage.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}
		}
		
		IEnumerator FadeOutGUI()
		{
			float startTime = Time.time;
			thisButton.enabled = false;
			StopCoroutine ("FadeInGUI");
//			collider.enabled = false;
			startColor = thisImage.color;
			targetColor = thisImage.color;
			targetColor.a = 0;
			
			while(thisImage.color.a > 0.001f)
			{
				thisImage.color = Color.Lerp (startColor, targetColor, Time.time - startTime);
				yield return null;
			}

			thisImage.color = targetColor;
			thisButton.enabled = false;
			this.gameObject.SetActive (false);
		}
    }
}