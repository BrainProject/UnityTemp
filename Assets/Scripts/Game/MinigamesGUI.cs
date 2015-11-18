using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

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
		public MinigamesGUIDetection guiDetection;
		public bool visible;
		public bool gsiStandalone;
		public bool clicked = false;

		void Awake()
		{
//			if (Application.loadedLevel > 1)
//				backIcon.gameObject.SetActive(true);
//			else
//				backIcon.gameObject.SetActive(false);
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

			gameSelectionIcon.thisButton.enabled = true;
			restartIcon.thisButton.enabled = true;
			if(!gsiStandalone)
				brainIcon.thisButton.enabled = true;
			guiDetection.guiIsHidden = false;

			gameSelectionIcon.show ();
			restartIcon.show ();
			if(!gsiStandalone)
				brainIcon.show ();

			if(MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject)
			{
				if (MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject.helpPrefab && !MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject.helpClone)
					showHelpIcon.show ();
			}

			MGC.Instance.TakeControlForGUIAction(true);
        }

        public void hide()
		{
			visible = false;
			gameSelectionIcon.hide ();
			restartIcon.hide ();
			if(!gsiStandalone)
				brainIcon.hide ();
			guiDetection.guiIsHidden = true;
			showHelpIcon.hide ();

			
			if(MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject)
			{
				if(!MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject.helpClone)
				{
					MGC.Instance.TakeControlForGUIAction(false);
				}
			}
        }

		void OnLevelWasLoaded (int level)
		{
			//handle back icon visibility
			if (Application.loadedLevel > 1)
				backIcon.gameObject.SetActive(true);
			else
				backIcon.gameObject.SetActive(false);
		}
    }
}