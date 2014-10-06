using UnityEngine;
using System.Collections;

namespace Game {
	public class DontDestroy : MonoBehaviour {

		// Use this for initialization
		void Awake()
		{
			if(GameObject.Find(this.name) != this.gameObject)
				Destroy (this.gameObject);
			DontDestroyOnLoad (this.gameObject);
		}
	}
}