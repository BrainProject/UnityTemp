using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class ChangeImage : MonoBehaviour {

		// Use this for initialization
		void Start () {
			renderer.material.color = Color.grey;
		}

		void OnMouseEnter() {
			renderer.material.color = Color.white;
		}

		void OnMouseExit() {
			renderer.material.color = Color.grey;
		}
	}
}