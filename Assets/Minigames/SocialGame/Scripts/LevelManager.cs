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
			GameObject Neuronek = MGC.Instance.neuronHelp;
			if (Neuronek)
			{
				Neuronek.GetComponent<Game.BrainHelp>().ShowSmile(Resources.Load("Neuron/smilyface") as Texture);
			}
			finish ();
		}

		/// <summary>
		/// Finish this level.
		/// </summary>
		public static void finish()
		{
			HelpListener.Instance.StopAll(true);
			KinectManagerSwitcher.deactivateThisLevelKManager();
			MGC.Instance.minigamesGUI.show(true);
			//Debug.LogError("finito");
		}
		#endif
	}
}