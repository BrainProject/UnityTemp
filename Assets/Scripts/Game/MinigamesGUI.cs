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
            rewardIcon.resetState();
            gameSelectionIcon.resetState();
            restartIcon.resetState();
			brainIcon.resetState();

			//StartCoroutine (rewardIcon.FadeInGUI ());
			gameSelectionIcon.gameObject.SetActive (true);
			restartIcon.gameObject.SetActive (true);
			brainIcon.gameObject.SetActive (true);
			guiDetection.guiIsHidden = false;

			gameSelectionIcon.show ();
			restartIcon.show ();
			brainIcon.show ();

            //rewardIcon.gameObject.SetActive(false);

			restartIcon.GetComponent<MinigamesGUIIconsActions>().SetRestartDifferentScene(differentRestartScene,differentRestartSceneName);
        }

        public void hide()
		{
			visible = false;
			gameSelectionIcon.hide ();
			restartIcon.hide ();
			brainIcon.hide ();
			guiDetection.guiIsHidden = true;
        }

		void OnLevelWasLoaded (int level)
		{
			visible = false;
			Color tmp = gameSelectionIcon.renderer.material.color;
			tmp.a = 0.01f;
			gameSelectionIcon.renderer.material.color = tmp;
			restartIcon.renderer.material.color = tmp;
			brainIcon.renderer.material.color = tmp;
			if(gameSelectionIcon.gameObject.activeSelf)
				gameSelectionIcon.hide ();
			if(restartIcon.gameObject.activeSelf)
				restartIcon.hide ();
			if(brainIcon.gameObject.activeSelf)
				brainIcon.hide ();
		}
    }

}