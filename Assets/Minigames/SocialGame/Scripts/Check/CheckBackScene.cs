using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckBackScene : CheckCancleFigure {

		protected override void EndTimer()
		{
			TimerReset ();
			activated = false;
			show ();
			MGC.Instance.minigamesGUI.backIcon.GUIAction ();
		}
	}
}
