/*
 * Created by: Milan Doležal
 */ 


using UnityEngine;
using System.Collections;

public class SweepCamera : MonoBehaviour {
	private bool OnTransition { get; set; }

	// Use this for initialization
	void Start()
	{
		OnTransition = false;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(Input.GetButtonDown("Horizontal"))
		{
			//TODO: Find nearest waypoint
			if(Input.GetAxis("Horizontal") < 0)
			{
				this.GetComponent<SmoothCameraMove>().Move = true;
				this.GetComponent<SmoothCameraMove>().From = this.transform.position;
				this.GetComponent<SmoothCameraMove>().To = GameObject.Find ("GreenPos").transform.position;
				GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition = GameObject.Find ("GreenPos").transform.position;
			}
			else if(Input.GetAxis("Horizontal") > 0)
			{
				this.GetComponent<SmoothCameraMove>().Move = true;
				this.GetComponent<SmoothCameraMove>().From = this.transform.position;
				this.GetComponent<SmoothCameraMove>().To = GameObject.Find ("OrangePos").transform.position;
				GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition = GameObject.Find ("OrangePos").transform.position;
			}
		}
	}
	void OnGUI()
	{
		if(GUI.Button(new Rect(20, 200, 100, 30), "Reset pos."))
		{
			this.GetComponent<SmoothCameraMove>().To = GameObject.Find ("BluePos").transform.position;
			GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition = GameObject.Find ("BluePos").transform.position;
		}
	}
}
