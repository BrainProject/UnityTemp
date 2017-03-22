using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace Game
{
    public class MinigamesGUI : MonoBehaviour
    {
        public MinigamesGUIIconsActions rewardIcon;
        public MinigamesGUIIconsActions gameSelectionIcon;
        public MinigamesGUIIconsActions restartIcon;
		public MinigamesGUIIconsActions brainIcon;
		public MinigamesGUIIconsActions backIcon;
		public MinigamesGUIIconsActions screenshotIcon;
		public MinigamesGUIIconsActions replayHelpIcon;
		public MinigamesGUIIconsActions hideHelpIcon;
		public MinigamesGUIIconsActions showHelpIcon;
		public MinigamesGUIIconsActions menuIcon;
		public MinigamesGUIDetection guiDetection;
		public bool visible;
		public bool gsiStandalone;
		public bool clicked = false;

        //		void Awake()
        //		{
        //			if (Application.loadedLevel > 1)
        //				backIcon.gameObject.SetActive(true);
        //			else
        //				backIcon.gameObject.SetActive(false);
        //		}

        private void Start()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        /// <summary>
        /// Shows minigames GUI.
        /// </summary>
        /// <param name="showReward">If set to <c>true</c>, shows reward button.</param>
        /// <param name="differentRestartScene">If set to <c>true</c>, attempts to load different scene for restart.</param>
        /// <param name="differentRestartSceneName">Scene name to be loaded with restart.</param>
        public void show(bool showReward = false, bool differentRestartScene = false, string differentRestartSceneName = "Main")
		{
			visible = true;


			switch (MGC.Instance.menuType) 
			{
			case MenuType.Brain:
				{
					gameSelectionIcon.thisButton.enabled = true;
					gameSelectionIcon.show ();
					brainIcon.thisButton.enabled = true;
					brainIcon.show ();
					break;
				}
			default:
				{
                        if (menuIcon)
                        {
                            if (menuIcon.thisButton)
                            {
                                menuIcon.thisButton.enabled = true;
                                menuIcon.show();
                            }
                            else
                            {
                                UnityEngine.Debug.LogWarning("No menuIcon button!");
                            }
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning("No menuIcon object!");
                        }
					break;
				}
			}

            if (restartIcon)
            {
                if (restartIcon.thisButton)
                {
                    restartIcon.thisButton.enabled = true;
                    restartIcon.show();
                }
            }
				
			guiDetection.guiIsHidden = false;

            if(MGC.Instance.neuronHelp)
            { 
			    if(MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject)
			    {
				    //if (MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject.helpPrefab && !MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject.helpClone)
					    //showHelpIcon.show ();
			    }
            }

			MGC.Instance.TakeControlForGUIAction(true);
        }

        public void hide()
		{
			visible = false;
			gameSelectionIcon.hide ();
			restartIcon.hide ();
			brainIcon.hide ();
			menuIcon.hide ();
			guiDetection.guiIsHidden = true;
			//showHelpIcon.hide ();

            screenshotIcon.hide();
            
            if (MGC.Instance.neuronHelp)
            {
                if (MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject)
                {
                    if (!MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject.helpClone)
                    {
                        MGC.Instance.TakeControlForGUIAction(false);
                    }
                }
            }
            else
            {
                MGC.Instance.TakeControlForGUIAction(false);
            }
        }

		void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            //handle back icon visibility
            if (scene.buildIndex > 0)
            {
                StopAllCoroutines();
                backIcon.gameObject.SetActive(true);
                backIcon.show();
                hide();
            }
            else
            {
                backIcon.gameObject.SetActive(false);
            }
		}
    }
}