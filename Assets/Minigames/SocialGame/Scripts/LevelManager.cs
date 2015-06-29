using UnityEngine;
using System.Collections;


namespace SocialGame{
	public class LevelManager : MonoBehaviour {
		#if UNITY_STANDALONE
		public static int gameSelected = 0; //1-player 2-player 0-nonselect

		/// <summary>
		/// win this level
		/// </summary>
		public static void win()
		{
			MGC.Instance.WinMinigame ();
			finish ();
		}

		/// <summary>
		/// Finish this level.
		/// </summary>
		public static void finish()
		{
			HelpListener.Instance.StopAll(true);
			KinectManagerSwitcher.deactivateThisLevelKManager();
			//Debug.LogError("finito");
		}
		#endif
	}
}