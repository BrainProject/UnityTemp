using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Menu : MonoBehaviour {

		public GameObject[] Icons;
		public float NumOfIconOnHeight;
		public Camera main;
		public float Border;

		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake () {
			Icon.Size = NumOfIconOnHeight;
			Icon.Expands = Border;
			Icon.MainCamera = main;
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		void Update () {
			Icon.Size = NumOfIconOnHeight;
			Icon.Expands = Border;
		}
	}
}
