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
		public LevelManagerSelection levelManager;
		public bool movingLeft;
		public bool movingRight;

		public bool ReadyToLeave;
		//public bool ReadyToLeave { get; set; }

		private bool OnTransition { get; set; }
#if UNITY_STANDALONE
        private float timestamp;
#endif

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
			//Set target position of camera back to its original point
			if((Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") < 0) || Input.GetMouseButtonDown(1))
			{
				ZoomOutCamera();
			}

            //print (currentWaypoint.name);
            //print ("Distance: " + Vector3.Distance (this.transform.position, currentWaypoint.transform.position));


#if UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))
			{
                MGC.Instance.initialTouchPosition.x = Input.mousePosition.x;
                timestamp = Time.time;
			}
            if (Time.time - timestamp > 0.5 && (MGC.Instance.initialTouchPosition.x != 0))
            {
                if (((MGC.Instance.initialTouchPosition.x - Input.mousePosition.x) > MGC.Instance.swipeDistance.x) && !movingLeft)
				{
					//Set current waypoint to left
					if(currentWaypoint.GetComponent<SelectionWaypoint>().left != null)
					{
						SelectionWaypoint next = currentWaypoint.GetComponent<SelectionWaypoint>().left.GetComponent<SelectionWaypoint>();
						while(next.skipOnAndroid && next != currentWaypoint.GetComponent<SelectionWaypoint>())
							next = next.left.GetComponent<SelectionWaypoint>();
						
						targetWaypoint = currentWaypoint = next.gameObject;
						mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
					}
                    movingLeft = true;
					movingRight = false;
					SetNewTarget();
				}
				else if(((MGC.Instance.initialTouchPosition.x - Input.mousePosition.x) < -MGC.Instance.swipeDistance.x) && !movingRight)
				{
					//Set current waypoint to right
					if(currentWaypoint.GetComponent<SelectionWaypoint>().right != null)
					{
						SelectionWaypoint next = currentWaypoint.GetComponent<SelectionWaypoint>().right.GetComponent<SelectionWaypoint>();
						while(next.skipOnAndroid && next != currentWaypoint.GetComponent<SelectionWaypoint>())
							next = next.right.GetComponent<SelectionWaypoint>();
						
						targetWaypoint = currentWaypoint = next.gameObject;
						mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
					}
					movingLeft = false;
					movingRight = true;
					SetNewTarget();
				}
                MGC.Instance.initialTouchPosition = Vector2.zero;
			}
#else
            if (Input.GetMouseButtonDown(0))
			{
                MGC.Instance.initialTouchPosition.x = Input.mousePosition.x;
			}
            if (Input.GetMouseButtonUp(0) && (MGC.Instance.initialTouchPosition.x != 0))
            {
                if (((MGC.Instance.initialTouchPosition.x - Input.mousePosition.x) < -MGC.Instance.swipeDistance.x) && !movingLeft)
				{
					//Set current waypoint to left
					if(currentWaypoint.GetComponent<SelectionWaypoint>().left != null)
					{
						SelectionWaypoint next = currentWaypoint.GetComponent<SelectionWaypoint>().left.GetComponent<SelectionWaypoint>();
						while(next.skipOnAndroid && next != currentWaypoint.GetComponent<SelectionWaypoint>())
							next = next.left.GetComponent<SelectionWaypoint>();
						
						targetWaypoint = currentWaypoint = next.gameObject;
						mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
					}
                    movingLeft = true;
					movingRight = false;
					SetNewTarget();
				}
				else if(((MGC.Instance.initialTouchPosition.x - Input.mousePosition.x) > MGC.Instance.swipeDistance.x) && !movingRight)
				{
					//Set current waypoint to right
					if(currentWaypoint.GetComponent<SelectionWaypoint>().right != null)
					{
						SelectionWaypoint next = currentWaypoint.GetComponent<SelectionWaypoint>().right.GetComponent<SelectionWaypoint>();
						while(next.skipOnAndroid && next != currentWaypoint.GetComponent<SelectionWaypoint>())
							next = next.right.GetComponent<SelectionWaypoint>();
						
						targetWaypoint = currentWaypoint = next.gameObject;
						mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
					}
					movingLeft = false;
					movingRight = true;
					SetNewTarget();
				}
                MGC.Instance.initialTouchPosition = Vector2.zero;
			}
#endif
            /*if(Input.GetButtonDown("Horizontal") || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.L))))
            {
                if((Input.GetAxis("Horizontal") < 0 || Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.J) && Input.GetMouseButton(0))) && !movingLeft)
                {
                    //Set current waypoint to left
                    if(currentWaypoint.GetComponent<SelectionWaypoint>().left != null)
                    {
                        SelectionWaypoint next = currentWaypoint.GetComponent<SelectionWaypoint>().left.GetComponent<SelectionWaypoint>();
                        while(next.skipOnStandalone && next != currentWaypoint.GetComponent<SelectionWaypoint>())
                            next = next.left.GetComponent<SelectionWaypoint>();

                        targetWaypoint = currentWaypoint = next.gameObject;
                        mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
                    }
                    movingLeft = true;
                    movingRight = false;
                    SetNewTarget();
                }
                else if((Input.GetAxis("Horizontal") > 0 || Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.L) && Input.GetMouseButton(0)))  && !movingRight)
                {
                    //Set current waypoint to right
                    if(currentWaypoint.GetComponent<SelectionWaypoint>().right != null)
                    {
                        SelectionWaypoint next = currentWaypoint.GetComponent<SelectionWaypoint>().right.GetComponent<SelectionWaypoint>();
                        while(next.skipOnStandalone && next != currentWaypoint.GetComponent<SelectionWaypoint>())
                            next = next.right.GetComponent<SelectionWaypoint>();

                        targetWaypoint = currentWaypoint = next.gameObject;
                        mgc.currentCameraDefaultPosition = currentWaypoint.transform.position;
                    }
                    movingLeft = false;
                    movingRight = true;
                    SetNewTarget();
                }
            }*/



            if (movingLeft || movingRight)
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
		
		public void ZoomOutCamera()
		{
			levelManager.OnSelection = false;
			levelManager.minigameOnSelection = null;
			GetComponent<SmoothCameraMove> ().MoveCameraBack ();
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
			if(levelManager.minigameOnSelection)
			{
				levelManager.OnSelection = false;
				levelManager.minigameOnSelection = null;
			}
			ReadyToLeave = true;
		}
	}
}