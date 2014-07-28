//using UnityEngine;
//using System.Collections;
//
//public class _XTestMouseHold : MonoBehaviour {
//	private Camera mainCamera;
//	private bool holding;
//
//	// Use this for initialization
//	void Start () {
//		holding = false;
//		mainCamera = GameObject.Find ("Main Camera").camera;
//	}
//	
//	// Update is called once per frame
//	void Update () {
////		Vector3 newPosition = GameObject.Find ("Main Camera").camera.ScreenToWorldPoint(Input.mousePosition;
////		print (newPosition);
////		this.transform.position = newPosition;
//	}
//	void OnMouseOver()
// 	{
//		if(Input.GetKey (KeyCode.C))
//			holding = !holding;
//		if(holding)
//			this.transform.position = new Vector3(GameObject.Find ("Main Camera").camera.ScreenToWorldPoint(Input.mousePosition).x, GameObject.Find ("Main Camera").camera.ScreenToWorldPoint(Input.mousePosition).y, -0.5f);
//	}
//
//	void OnMouseExit()
//	{
//		holding = false;
//		this.transform.position = new Vector3 (-5.8f, 0.5f, -0.5f);
//	}
//}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _XTestMouseHold: MonoBehaviour {
	private Color OriginalColor { get; set; }
	private Vector3 tmp;
	private bool picked;
	
	void Start()
	{
		OriginalColor = this.renderer.material.color;
		picked = false;
	}
	
	
	public void Update()
	{
		if (picked)
		{
			tmp = Camera.main.WorldToScreenPoint (transform.position);
			tmp.x = Input.mousePosition.x;
			tmp.y = Input.mousePosition.y;
			tmp = Camera.main.ScreenToWorldPoint (tmp);
			tmp.z = 0;
			transform.position = tmp;
		}
	}
	
	void OnMouseEnter()
	{
		this.renderer.material.color = Color.green;
	}
	
	void OnMouseExit()
	{
		if(!picked)
			this.renderer.material.color = OriginalColor;
	}
	
	void OnMouseOver()
	{
		if(Input.GetKey (KeyCode.C))
		{
			this.rigidbody.useGravity = !this.rigidbody.useGravity;
			picked = !picked;
		}
	}
}

