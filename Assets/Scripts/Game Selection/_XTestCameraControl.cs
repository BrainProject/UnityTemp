/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;
//using Game;

namespace MinigameSelection 
{
	public class _XTestCameraControl : MonoBehaviour 
    {
		public float sweepSpeed = 1.0f;
		public GameObject currentWaypoint;
		public GameObject nextWaypoint;
		public int direction;

		public bool ReadyToLeave;
		//public bool ReadyToLeave { get; set; }

		private bool OnTransition { get; set; }
		
        private MGC mgc;

		void Start()
		{
			direction = 0;
			print ("Initial waypoint is: " + currentWaypoint.name);
			OnTransition = false;
            mgc = MGC.Instance;
			this.transform.position = mgc.currentCameraDefaultPosition;
			this.transform.rotation = currentWaypoint.transform.rotation;
			//this.transform.position = currentWaypoint.transform.position;
		}
		
		void Update()
		{
			//print (currentWaypoint.name);
			//print ("Distance: " + Vector3.Distance (this.transform.position, currentWaypoint.transform.position));
			if(Input.GetButtonDown("Horizontal"))
			{
				if(Input.GetAxis("Horizontal") < 0)
				{
					if(direction >= -6)
						--direction;
					//Set current waypoint to left
					if(currentWaypoint.GetComponent<SelectionWaypoint>().left != null)
					{
						currentWaypoint = currentWaypoint.GetComponent<SelectionWaypoint>().left;
						mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
					}
				}
				else if(Input.GetAxis("Horizontal") > 0)
				{
					if(direction <= 6)
						++direction;
					//Set current waypoint to right
					if(currentWaypoint.GetComponent<SelectionWaypoint>().right != null)
					{
						currentWaypoint = currentWaypoint.GetComponent<SelectionWaypoint>().right;
						mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
					}
				}
				this.GetComponent<SmoothCameraMove>().Move = true;
				this.GetComponent<SmoothCameraMove>().Speed = sweepSpeed;
				this.GetComponent<SmoothCameraMove>().From = this.transform.position;
				this.GetComponent<SmoothCameraMove>().To = currentWaypoint.transform.position;
				this.GetComponent<SmoothCameraMove>().FromYRot = this.transform.eulerAngles.y;
				this.GetComponent<SmoothCameraMove>().ToYRot = currentWaypoint.transform.eulerAngles.y;
//				if(GameObject.Find("_LevelManager").GetComponent<LevelManagerSelection>().minigameOnSelection)
//				{
//					GameObject.Find("_LevelManager").GetComponent<LevelManagerSelection>().minigameOnSelection.GetComponent<SelectMinigame>().OnSelection = false;
//					GameObject.Find("_LevelManager").GetComponent<LevelManagerSelection>().minigameOnSelection = null;
//				}
			}
		}

		void OnGUI()
		{
			if(GUI.Button(new Rect(20, 200, 100, 30), "Reset pos"))
			{
				currentWaypoint = GameObject.Find ("OccipitalLobePos");
				this.GetComponent<SmoothCameraMove>().From = GameObject.Find ("OccipitalLobePos").transform.position;
				this.GetComponent<SmoothCameraMove>().To = GameObject.Find ("OccipitalLobePos").transform.position;
				this.transform.position = currentWaypoint.transform.position;
			}
		}

		public void BackToMain()
		{
			if(ReadyToLeave)
			{
				//StartCoroutine(GameObject.Find ("LoadLevelWithFade").GetComponent<LoadLevelWithFade>().LoadMainLevel(false, "Main"));
                mgc.sceneLoader.LoadScene("Main");

				mgc.fromSelection = true;
			}
		}
	}
}