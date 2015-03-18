/// <summary>
/// Icon settings.
/// </summary>

using UnityEngine;
using System.Collections;

namespace MinigameSelection 
{
	public class IconSettings : MonoBehaviour {

		void Awake ()
		{
			this.transform.parent.GetComponent<SelectMinigame> ().Icon = this.gameObject;
			this.transform.localScale = new Vector3 (0.6f, 0.6f, 1);
		}
	}
}