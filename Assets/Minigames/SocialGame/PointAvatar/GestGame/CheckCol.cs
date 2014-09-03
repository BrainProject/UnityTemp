using UnityEngine;
using System.Collections;

public class CheckCol : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnColisionEnter(Collision col)
	{
		Debug.Log("colision");
	}
}
