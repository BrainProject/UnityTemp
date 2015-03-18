using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Menu : MonoBehaviour {

		public GameObject[] Icons;
		public float NumOfIconOnHeight;
		public Camera main;
		public float Border;

		// Use this for initialization
		void Awake () {
			Icon.Size = NumOfIconOnHeight;
			Icon.Expands = Border;
			Icon.MainCamera = main;
		}

		void Update () {
			Icon.Size = NumOfIconOnHeight;
			Icon.Expands = Border;
		}
	}
}
