using UnityEngine;
using System.Collections;

namespace Game
{
	public class NEWBrainHelp : MonoBehaviour {
		public HelpVisibility helpObject;

		void OnMouseDown()
		{
			helpObject.ShowHelpAnimation ();
		}
	}
}