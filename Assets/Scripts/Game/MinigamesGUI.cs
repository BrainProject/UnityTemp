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
		public MinigamesGUIDetection guiDetection;
		public bool visible;
		public bool gsiStandalone;
		public bool clicked = false;

		void Awake()
		{
			if (Application.loadedLevel > 1)
				backIcon.gameObject.SetActive(true);
			else
				backIcon.gameObject.SetActive(false);
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
			if (Application.loadedLevel > 1)
				backIcon.gameObject.SetActive(true);
			else
				backIcon.gameObject.SetActive(false);
		}
    }
}