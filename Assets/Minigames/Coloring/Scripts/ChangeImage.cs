using UnityEngine;
using System.Collections;

public class ChangeImage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		renderer.material.color = Color.grey;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		renderer.material.color = Color.white;
	}

	void OnMouseExit() {
		renderer.material.color = Color.grey;
	}
}
