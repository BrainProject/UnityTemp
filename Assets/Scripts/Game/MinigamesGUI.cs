using UnityEngine;
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
		public MinigamesGUIDetection guiDetection;
		public bool visible;
		public bool gsiStandalone;
		public bool clicked = false;

		/// <summary>
		/// Shows minigames GUI.
		/// </summary>
		/// <param name="showReward">If set to <c>true</c>, shows reward button.</param>
		/// <param name="differentRestartScene">If set to <c>true</c>, attempts to load different scene for restart.</param>
		/// <param name="differentRestartSceneName">Scene name to be loaded with restart.</param>
        
		public void show(bool showReward = false, bool differentRestartScene = false, string differentRestartSceneName = "Main")
		{
			visible = true;
            //reset state of all icons
            //rewardIcon.resetState();
            //gameSelectionIcon.resetState();
            //restartIcon.resetState();
			//brainIcon.resetState();

			//StartCoroutine (rewardIcon.FadeInGUI ());
			gameSelectionIcon.thisButton.enabled = true;
			restartIcon.thisButton.enabled = true;
			if(!gsiStandalone)
				brainIcon.thisButton.enabled = true;
			guiDetection.guiIsHidden = false;

			gameSelectionIcon.show ();
			restartIcon.show ();
			if(!gsiStandalone)
				brainIcon.show ();

            //rewardIcon.gameObject.SetActive(false);
        }

        public void hide()
		{
			visible = false;
			gameSelectionIcon.hide ();
			restartIcon.hide ();
			if(!gsiStandalone)
				brainIcon.hide ();
			guiDetection.guiIsHidden = true;
        }

		void OnLevelWasLoaded (int level)
		{
//			visible = false;
//			Color tmp = gameSelectionIcon.thisImage.color;
//			tmp.a = 0.01f;
//			gameSelectionIcon.thisImage.color = tmp;
//			restartIcon.thisImage.color = tmp;
//			brainIcon.thisImage.color = tmp;
//			if(gameSelectionIcon.gameObject.activeSelf)
//				gameSelectionIcon.hide ();
//			if(restartIcon.gameObject.activeSelf)
//				restartIcon.hide ();
//			if(brainIcon.gameObject.activeSelf)
//				brainIcon.hide ();
		}
    }

}