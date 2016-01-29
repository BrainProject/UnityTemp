using UnityEngine;
using System.Collections;

namespace SocialGame{

	public enum GUIActionSocialGame
	{
		None = 0,
		Back,
		Help
	}

	public class CheckGUIControls : CheckCancleFigure {
		public GUIActionSocialGame type = GUIActionSocialGame.None; 
        #if UNITY_STANDALONE
		protected override void EndTimer()
		{
			TimerReset ();
			//activated = false;
			//show ();
			switch (type) {
			case(GUIActionSocialGame.Back):
				MGC.Instance.minigamesGUI.backIcon.GUIAction ();
				break;
			case(GUIActionSocialGame.Help):
				Game.NEWBrainHelp tempHelp = MGC.Instance.neuronHelp.GetComponent<Game.NEWBrainHelp> ();
				if (tempHelp)
                {
					tempHelp.helpObject.ShowHelpAnimation ();
				}
				break;
			}

		}
        #endif
    }
}
