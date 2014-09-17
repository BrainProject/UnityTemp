using UnityEngine;
using System.Collections;


#if UNITY_STANDALONE
namespace SocialGame{
	public class LevelManager : MonoBehaviour {
		public static int gameSelected = 0; //1-player 2-player 0-nonselect

		public static void finish()
		{
			KinectManagerSwitcher.deactivateThisLevelKManager();
			MGC.Instance.minigamesGUI.show(true);
			//Debug.LogError("finito");
		}
		
	}
}
#endif