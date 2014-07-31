using UnityEngine;
using System.Collections;

public class ControlsDraftFPS : MonoBehaviour {

	private int speed = 5;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("w"))
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
		if(Input.GetKey("s"))
			transform.Translate(Vector3.forward * Time.deltaTime * -speed);
	
	}
}
