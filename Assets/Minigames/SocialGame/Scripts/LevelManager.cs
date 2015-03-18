using UnityEngine;
using System.Collections;


namespace SocialGame{
	public class LevelManager : MonoBehaviour {
		#if UNITY_STANDALONE
		public static int gameSelected = 0; //1-player 2-player 0-nonselect

		public static void win()
		{
			GameObject Neuronek = MGC.Instance.neuronHelp;
			if (Neuronek)
			{
				Neuronek.GetComponent<Game.BrainHelp>().ShowSmile(Resources.Load("Neuron/smilyface") as Texture);
			}
			finish ();
		}

		public static void finish()
		{
			KinectManagerSwitcher.deactivateThisLevelKManager();
			MGC.Instance.minigamesGUI.show(true);
			//Debug.LogError("finito");
		}
		#endif
	}
}