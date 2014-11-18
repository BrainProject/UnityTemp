/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;
using Game;

namespace MinigameSelection 
{
	public class SelectMinigame : MonoBehaviour 
    {
		public string minigameName;
		public float cameraDistance = 5;
		public Texture minigameIcon;
		public Texture minigameHelp;
		public bool kinectRequired = false;

		//Will be changed according to currently selected brain part.
		//public Vector3 CameraDefaultPosition { get; set; }
		
		public bool OnSelection { get; set; }
		private bool MouseHover{ get; set; }
		private Vector3 CameraZoom { get; set; }
		private LevelManagerSelection LevelManager { get; set; }
		private GameObject Icon { get; set; }
		private Color OriginalColor { get; set; }
		private Vector3 OriginalIconScale { get; set; }

		void Start()
		{
			LevelManager = GameObject.Find ("_LevelManager").GetComponent<LevelManagerSelection>();
			OnSelection = false;
			//CameraDefaultPosition = MGC.Instance.currentCameraDefaultPosition;
			//CameraZoom = this.transform.position - this.transform.forward;
			MouseHover = false;
			if(minigameIcon)
			{
				Icon = (GameObject)Instantiate(GameObject.Find ("Selection Part Icon"));
				Icon.renderer.material.mainTexture = minigameIcon;
				Icon.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				Icon.transform.parent = this.transform;
				OriginalIconScale = Icon.transform.localScale;
				//Icon.renderer.material.color = new Color(Icon.renderer.material.color.r, Icon.renderer.material.color.g, Icon.renderer.material.color.b, 0);
			}

            if(MGC.Instance.minigameStates.GetPlayed(minigameName))
            {
                (this.GetComponent("Halo") as Behaviour).enabled = true;
				this.renderer.material.color = new Color(this.renderer.material.color.r + 1f, this.renderer.material.color.g + 1f, this.renderer.material.color.b + 1f);
            }
			
            if(minigameName == "")
			{
				this.renderer.material.color = Color.gray;
				this.GetComponent<SelectMinigame>().enabled = false;
				this.collider.enabled = false;
				//this.gameObject.SetActive(false);
			}

/*			if(MGC.Instance.selectedMinigame == this.gameObject.transform.position)
			{
				OnSelection = true;
				LevelManager.minigameOnSelection = this.gameObject;
				CameraZoom = MGC.Instance.currentCameraDefaultPosition;
				MGC.Instance.selectedMinigame = Vector3.zero;
				//Camera.main.GetComponent<CameraDefaultPosition
			}
*/		}

		void Update()
		{
			if(Icon)
			{
				Icon.transform.position = this.transform.position;
				Vector3 dir = Camera.main.transform.position - this.transform.position;
				dir.Normalize();
				dir = dir * this.transform.lossyScale.x*0.75f;
				Icon.transform.position += dir;
			}
//            if (Input.GetButtonDown("Fire1") && !MouseHover)
//            {
//                OnSelection = false;
//            }

			//Set target position of camera back to its original point
			if((Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") < 0) || Input.GetMouseButtonDown(1))
			{
				OnSelection = false;
				LevelManager.minigameOnSelection = null;
//				//StartCoroutine(mainCamera.GetComponent<SmoothCameraMove>().CameraLerp(Time.time));
//				mainCamera.GetComponent<SmoothCameraMove>().Move = true;
//				mainCamera.GetComponent<SmoothCameraMove>().Speed = mainCamera.GetComponent<SmoothCameraMove>().defaultSpeed;
//				mainCamera.GetComponent<SmoothCameraMove>().From = mainCamera.transform.position;
//				mainCamera.GetComponent<SmoothCameraMove>().To = mainCamera.GetComponent<SweepCamera>().currentWaypoint.transform.position;
//				//mainCamera.GetComponent<SmoothCameraMove>().To = GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition;
			}
		}

		
		void OnMouseEnter()
		{
			OriginalColor = this.renderer.material.color;
			this.renderer.material.color = new Color(OriginalColor.r + 0.4f, OriginalColor.g + 0.4f, OriginalColor.b + 0.4f);
//			Texture tmp = (Texture)Resources.Load ("Selection/" + iconName, typeof(Texture));
//			if(tmp)
//			{
//				Icon.renderer.material.mainTexture = tmp;
//				Icon.renderer.material.color = new Color(Icon.renderer.material.color.r, Icon.renderer.material.color.g, Icon.renderer.material.color.b, 1);
//				Icon.transform.position = this.transform.position;
//			}
			if(Icon)
				Icon.transform.localScale = new Vector3 (0.6f, 0.6f, 1);
			//this.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
			MouseHover = true;

	        MGC.Instance.logger.addEntry("Mouse enter the object: '" + this.name + "'");
		}

		void OnMouseExit()
		{
			this.renderer.material.color = OriginalColor;
			//Icon.renderer.material.mainTexture = null;
			//Icon.renderer.material.color = new Color(Icon.renderer.material.color.r, Icon.renderer.material.color.g, Icon.renderer.material.color.b, 0);
			MouseHover = false;
			if(Icon)
				Icon.transform.localScale = OriginalIconScale;
	        MGC.Instance.logger.addEntry("Mouse exit the object: '" + this.name + "'");
		}

		void OnMouseUp()
		{
			if(!Camera.main.GetComponent<CameraControl>().movingLeft && !Camera.main.GetComponent<CameraControl>().movingRight)
			{
				//load mini-game if zooming or zoomed
				if(OnSelection)
				{
					//check if Kinect is connected
					if(kinectRequired)
					{
						print("Kinect is required for this game.");
						if(!MGC.Instance.kinectManager.activeSelf)
							return;
					}
					//GameObject.Find ("LoadLevelWithFade").guiTexture.enabled = true;
					//GameObject.Find ("_GameManager").GetComponent<GameManager>().selectedMinigame = this.gameObject;
					//if(minigameHelp && MGC.Instance.neuronHelp)
						MGC.Instance.neuronHelp.GetComponent<BrainHelp>().helpTexture = minigameHelp;
					//MGC.Instance.currentCameraDefaultPosition = CameraZoom;
	                MGC.Instance.currentBrainPart = this.transform.parent.GetComponent<BrainPart>().brainPart;
					MGC.Instance.selectedMinigame = this.gameObject.transform.position;
	                MGC.Instance.sceneLoader.LoadScene(minigameName);
				}

				//set target position of camera near to mini-game bubble
				else
				{
					GameObject cameraPoint = new GameObject("cameraPoint");
					cameraPoint.transform.parent = this.transform;
					cameraPoint.transform.localPosition = new Vector3(0, 0, cameraDistance);

					CameraZoom = cameraPoint.transform.position;
					//StartCoroutine(mainCamera.GetComponent<SmoothCameraMove>().CameraLerp(Time.time));
					//Camera.main.GetComponent<CameraControl>().currentWaypoint = this.transform.parent.gameObject;
					Camera.main.GetComponent<CameraControl>().currentWaypoint = this.transform.parent.GetComponent<BrainPart>().waypoint;
					Camera.main.GetComponent<CameraControl>().targetWaypoint = this.transform.parent.GetComponent<BrainPart>().waypoint;
					Camera.main.GetComponent<SmoothCameraMove>().Move = true;
					Camera.main.GetComponent<SmoothCameraMove>().From = Camera.main.transform.position;
					Camera.main.GetComponent<SmoothCameraMove>().FromYRot = Camera.main.transform.eulerAngles.y;
					Camera.main.GetComponent<SmoothCameraMove>().Speed = Camera.main.GetComponent<SmoothCameraMove>().defaultSpeed;
					Camera.main.GetComponent<SmoothCameraMove>().To = CameraZoom;
					Camera.main.GetComponent<SmoothCameraMove>().ToYRot = Camera.main.GetComponent<CameraControl>().currentWaypoint.transform.eulerAngles.y;
					Camera.main.GetComponent<CameraControl>().ReadyToLeave = false;
					Camera.main.GetComponent<CameraControl>().movingLeft = false;
					Camera.main.GetComponent<CameraControl>().movingRight = false;

					//deactivate OnSelection flag on previously zoomed minigame and set it to current minigame
					//if this is the first selected minigame, set it instantly
					if(LevelManager.minigameOnSelection == null)
						LevelManager.minigameOnSelection = this.gameObject;
					else
					{
						if(LevelManager.minigameOnSelection != this.gameObject)
							LevelManager.minigameOnSelection.GetComponent<SelectMinigame>().OnSelection = false;
						LevelManager.minigameOnSelection = this.gameObject;
					}
					OnSelection = true;
				}
			}
		}
	}
}