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

		/// <summary>
		/// Shows minigames GUI.
		/// </summary>
		/// <param name="showReward">If set to <c>true</c>, shows reward button.</param>
		/// <param name="differentRestartScene">If set to <c>true</c>, attempts to load different scene for restart.</param>
		/// <param name="differentRestartSceneName">Scene name to be loaded with restart.</param>
        
		public void show(bool showReward = false, bool differentRestartScene = false, string differentRestartSceneName = "Main")
        {
            //reset state of all icons
            rewardIcon.resetState();
            gameSelectionIcon.resetState();
            restartIcon.resetState();
			brainIcon.resetState();

            gameObject.SetActive(true);
            rewardIcon.gameObject.SetActive(false);

			restartIcon.GetComponent<MinigamesGUIIconsActions>().SetRestartDifferentScene(differentRestartScene,differentRestartSceneName);
        }

        public void hide()
        {
            gameObject.SetActive(false);
        }

		void OnLevelWasLoaded (int level)
		{
			gameObject.SetActive(false);
		}
    }

}