using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckBackScene : CheckCancleFigure {
        #if UNITY_STANDALONE
		protected override void EndTimer()
		{
			TimerReset ();
			activated = false;
			show ();
			MGC.Instance.minigamesGUI.backIcon.GUIAction ();
		}
        #endif
    }
}
