/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;
//using Game;

namespace MinigameSelection 
{
	public class CameraControl : MonoBehaviour 
    {
		public float sweepSpeed = 1.0f;
		public GameObject currentWaypoint;
		public GameObject targetWaypoint;
		public bool movingLeft;
		public bool movingRight;

		public bool ReadyToLeave;
		//public bool ReadyToLeave { get; set; }

		private bool OnTransition { get; set; }
		
        private MGC mgc;

		void Start()
		{
			print ("Initial waypoint is: " + currentWaypoint.name);
			OnTransition = false;
            mgc = MGC.Instance;
			this.transform.position = mgc.currentCameraDefaultPosition;
			this.transform.rotation = currentWaypoint.transform.rotation;
			this.transform.position = currentWaypoint.transform.position;
		}
		
		void Update()
		{
			//print (currentWaypoint.name);
			//print ("Distance: " + Vector3.Distance (this.transform.position, currentWaypoint.transform.position));
			if(Input.GetButtonDown("Horizontal"))
			{
				if(Input.GetAxis("Horizontal") < 0 && !movingLeft)
				{
					//Set current waypoint to left
					if(currentWaypoint.GetComponent<SelectionWaypoint>().left != null)
					{
						targetWaypoint = currentWaypoint = currentWaypoint.GetComponent<SelectionWaypoint>().left;
						mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
					}
					movingLeft = true;
					movingRight = false;
				}
				else if(Input.GetAxis("Horizontal") > 0 && !movingRight)
				{
					//Set current waypoint to right
					if(currentWaypoint.GetComponent<SelectionWaypoint>().right != null)
					{
						targetWaypoint = currentWaypoint = currentWaypoint.GetComponent<SelectionWaypoint>().right;
						mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
					}
					movingLeft = false;
					movingRight = true;
				}
				SetNewTarget();
			}

			if(movingLeft || movingRight)
			{
				if(Vector3.Distance(currentWaypoint.transform.position,this.transform.position) < 0.01f)
				{
					if(currentWaypoint != targetWaypoint)
					{
						if(movingLeft)
							currentWaypoint = currentWaypoint.GetComponent<SelectionWaypoint>().left;
						if(movingRight)
							currentWaypoint = currentWaypoint.GetComponent<SelectionWaypoint>().right;
						SetNewTarget();
					}
					else
					{
						movingLeft = false;
						movingRight = false;
					}
				}
			}

			//Debug actions
			if(Input.GetKeyDown(KeyCode.Keypad0)) //Reset position
			{
				currentWaypoint = GameObject.Find ("OccipitalLobePos");
				this.GetComponent<SmoothCameraMove>().From = currentWaypoint.transform.position;
				this.GetComponent<SmoothCameraMove>().To = currentWaypoint.transform.position;
				this.GetComponent<SmoothCameraMove>().FromYRot = currentWaypoint.transform.eulerAngles.y;
				this.GetComponent<SmoothCameraMove>().ToYRot = currentWaypoint.transform.eulerAngles.y;
				this.transform.position = currentWaypoint.transform.position;
				this.transform.rotation = currentWaypoint.transform.rotation;
				movingLeft = false;
				movingRight = false;
			}
		}

		void OnGUI()
		{
//			if(GUI.Button(new Rect(20, 20, 100, 30), "Reset pos"))
//			{
//				currentWaypoint = GameObject.Find ("OccipitalLobePos");
//				this.GetComponent<SmoothCameraMove>().From = currentWaypoint.transform.position;
//				this.GetComponent<SmoothCameraMove>().To = currentWaypoint.transform.position;
//				this.GetComponent<SmoothCameraMove>().FromYRot = currentWaypoint.transform.eulerAngles.y;
//				this.GetComponent<SmoothCameraMove>().ToYRot = currentWaypoint.transform.eulerAngles.y;
//				this.transform.position = currentWaypoint.transform.position;
//				this.transform.rotation = currentWaypoint.transform.rotation;
//				movingLeft = false;
//				movingRight = false;
//			}
			GUI.Label (new Rect (20, 20, 200, 40), "Map function\nprototype:");
			if(GUI.Button(new Rect(20, 60, 100, 30), "Occipital"))
			{
				targetWaypoint = GameObject.Find ("OccipitalLobePos");
				FindShorterDirectionToWaypoint();
				SetNewTarget();
			}
			if(GUI.Button(new Rect(20, 100, 100, 30), "Temporal"))
			{
				targetWaypoint = GameObject.Find ("TemporalLobePos");
				FindShorterDirectionToWaypoint();
				SetNewTarget();
			}
			if(GUI.Button(new Rect(20, 140, 100, 30), "Frontal"))
			{
				targetWaypoint = GameObject.Find ("FrontalLobePos");
				FindShorterDirectionToWaypoint();
				SetNewTarget();
			}
			if(GUI.Button(new Rect(20, 180, 100, 30), "Parietal"))
			{
				targetWaypoint = GameObject.Find ("ParietalLobePos");
				FindShorterDirectionToWaypoint();
				SetNewTarget();
			}
			if(GUI.Button(new Rect(20, 220, 100, 30), "Cerebellum"))
			{
				targetWaypoint = GameObject.Find ("CerebellumPos");
				FindShorterDirectionToWaypoint();
				SetNewTarget();
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

		public void FindShorterDirectionToWaypoint()
		{
			GameObject leftWaypoint = currentWaypoint;//.GetComponent<SelectionWaypoint>().left;
			GameObject rightWaypoint = currentWaypoint;//.GetComponent<SelectionWaypoint>().right;
			while(true)
			{
				if(currentWaypoint == targetWaypoint)
					return;
				leftWaypoint = leftWaypoint.GetComponent<SelectionWaypoint>().left;
				rightWaypoint = rightWaypoint.GetComponent<SelectionWaypoint>().right;
				if(leftWaypoint == targetWaypoint)
				{
					movingRight = false;
					movingLeft = true;
					currentWaypoint = currentWaypoint.GetComponent<SelectionWaypoint>().left;
					mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
					return;
				}
				if(rightWaypoint == targetWaypoint)
				{
					movingRight = true;
					movingLeft = false;
					currentWaypoint = currentWaypoint.GetComponent<SelectionWaypoint>().right;
					mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
					return;
				}
			}
		}

		public void SetNewTarget()
		{
			this.GetComponent<SmoothCameraMove>().Move = true;
			this.GetComponent<SmoothCameraMove>().Speed = sweepSpeed;
			this.GetComponent<SmoothCameraMove>().From = this.transform.position;
			this.GetComponent<SmoothCameraMove>().To = currentWaypoint.transform.position;
			this.GetComponent<SmoothCameraMove>().FromYRot = this.transform.eulerAngles.y;
			this.GetComponent<SmoothCameraMove>().ToYRot = currentWaypoint.transform.eulerAngles.y;
			if(GameObject.Find("_LevelManager").GetComponent<LevelManagerSelection>().minigameOnSelection)
			{
				GameObject.Find("_LevelManager").GetComponent<LevelManagerSelection>().minigameOnSelection.GetComponent<SelectMinigame>().OnSelection = false;
				GameObject.Find("_LevelManager").GetComponent<LevelManagerSelection>().minigameOnSelection = null;
			}
			ReadyToLeave = true;
		}
	}
}