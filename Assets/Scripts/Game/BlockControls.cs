using UnityEngine;
using System.Collections;

namespace Game
{
	public class BlockControls : MonoBehaviour {

		// Use this for initialization
		void Start () {
			this.transform.parent = Camera.main.transform;
			this.transform.localPosition = new Vector3 (0, 0, 0.5f);
		}
	}
}