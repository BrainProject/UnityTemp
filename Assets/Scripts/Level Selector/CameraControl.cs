/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;
using Game;

namespace MinigameSelection {
	public class CameraControl : MonoBehaviour {
		public float sweepSpeed = 1.0f;
		public GameObject currentWaypoint;

		private bool OnTransition { get; set; }
		internal bool ReadyToLeave { get; set; }

		// Use this for initialization
		void Start()
		{
			print (currentWaypoint.name);
			OnTransition = false;
			this.transform.position = GameObject.Find ("_GameManager").GetComponent<GameManager> ().currentCameraDefaultPosition;
			//this.transform.position = currentWaypoint.transform.position;
		}
		
		// Update is called once per frame
		void Update()
		{
			//print (currentWaypoint.name);
			//print ("Distance: " + Vector3.Distance (this.transform.position, currentWaypoint.transform.position));
			if(Input.GetButtonDown("Horizontal"))
			{
				if(Input.GetAxis("Horizontal") < 0)
				{
					//Set current waypoint to left
					if(currentWaypoint.GetComponent<DefaultCameraPosition>().left != null)
					{
						currentWaypoint = currentWaypoint.GetComponent<DefaultCameraPosition>().left;
						GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition = 
							currentWaypoint.transform.position;
					}
				}
				else if(Input.GetAxis("Horizontal") > 0)
				{
					//Set current waypoint to right
					if(currentWaypoint.GetComponent<DefaultCameraPosition>().right != null)
					{
						currentWaypoint = currentWaypoint.GetComponent<DefaultCameraPosition>().right;
						GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition = 
							currentWaypoint.transform.position;
					}
				}
				this.GetComponent<SmoothCameraMove>().Move = true;
				this.GetComponent<SmoothCameraMove>().Speed = sweepSpeed;
				this.GetComponent<SmoothCameraMove>().From = this.transform.position;
				this.GetComponent<SmoothCameraMove>().To = currentWaypoint.transform.position;
			}
		}
		void OnGUI()
		{
			if(GUI.Button(new Rect(20, 200, 100, 30), "Reset pos"))
			{
				currentWaypoint = GameObject.Find ("BluePos");
				this.GetComponent<SmoothCameraMove>().From = GameObject.Find ("BluePos").transform.position;
				this.GetComponent<SmoothCameraMove>().To = GameObject.Find ("BluePos").transform.position;
				this.transform.position = currentWaypoint.transform.position;
			}
		}

		public void BackToMain()
		{
			if(ReadyToLeave)
			{
				StartCoroutine(GameObject.Find ("LoadLevelWithFade").GetComponent<LoadLevelWithFade>().LoadSeledctedLevelWithColorLerp(false, "NewMain"));
				GameObject.Find("_GameManager").GetComponent<GameManager>().fromSelection = true;
			}
		}
	}
}