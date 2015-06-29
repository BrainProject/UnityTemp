/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;
using Game;
using UnityEngine.EventSystems;

namespace MinigameSelection 
{
	public class SelectMinigame : MonoBehaviour 
    {
		public string minigameName;
		public float cameraDistance = 5;
		public Texture minigameIcon;
		public GameObject minigameHelp;
		public bool kinectRequired = false;
		public LevelManagerSelection levelManager;

		//Will be changed according to currently selected brain part.
		//public Vector3 CameraDefaultPosition { get; set; }
		public GameObject Icon { get; set; }

		private bool MouseHover{ get; set; }
		private Vector3 CameraZoom { get; set; }
		private Color OriginalColor { get; set; }
		private Vector3 OriginalIconScale { get; set; }

		void Start()
		{
			MouseHover = false;

			if(minigameName == "")
			{
				this.renderer.material.color = Color.gray;
				this.GetComponent<SelectMinigame>().enabled = false;
				this.collider.enabled = false;
			}

#if !UNITY_STANDALONE
			if(kinectRequired)
			{
				this.renderer.material.color = Color.gray;
				this.GetComponent<SelectMinigame>().enabled = false;
				this.collider.enabled = false;
			}
#endif

			if(Icon)
			{
				Icon.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				OriginalIconScale = Icon.transform.localScale;
			}

            if(MGC.Instance.minigamesProperties.GetPlayed(minigameName))
            {
                (this.GetComponent("Halo") as Behaviour).enabled = true;
				this.renderer.material.color = new Color(this.renderer.material.color.r + 1f, this.renderer.material.color.g + 1f, this.renderer.material.color.b + 1f);
            }
		}

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
		}

		
		void OnMouseEnter()
		{
			OriginalColor = this.renderer.material.color;
			this.renderer.material.color = new Color(OriginalColor.r + 0.4f, OriginalColor.g + 0.4f, OriginalColor.b + 0.4f);

            if (Icon)
            {
                StartCoroutine("SmoothScaleUp");
            }

            MouseHover = true;
	        MGC.Instance.logger.addEntry("Mouse enter the object: '" + this.name + "'");
		}

		void OnMouseExit()
		{
			this.renderer.material.color = OriginalColor;
			MouseHover = false;

            if (Icon)
            {
                StartCoroutine("SmoothScaleDown");
            }

            MGC.Instance.logger.addEntry("Mouse exit the object: '" + this.name + "'");
		}

		void OnMouseUp()
		{
			if(!Camera.main.GetComponent<CameraControl>().movingLeft && !Camera.main.GetComponent<CameraControl>().movingRight && !EventSystem.current.IsPointerOverGameObject())
			{
				//load mini-game if zooming or zoomed
				if(levelManager.OnSelection && levelManager.minigameOnSelection == this.gameObject)
				{
					//check if Kinect is connected
					if(kinectRequired)
					{
						print("Kinect is required for this game.");
						if(!MGC.Instance.kinectManagerObject.activeSelf)
						{
							levelManager.FadeInOutKinectIcon();
							return;
						}
//						Kinect.KinectInterop.changeAngle = false;
					}

      //              if (MGC.Instance.neuronHelp)
    //                {
//                        MGC.Instance.neuronHelp.GetComponent<NEWBrainHelp>().helpObject.helpPrefab = minigameHelp;
  //                  }

                    MGC.Instance.currentBrainPart = this.transform.parent.GetComponent<BrainPart>().brainPart;
	                
                    //start the mini-game - choose difficulty (if applicable) and then load first scene of mini-game
                    MGC.Instance.startMiniGame(minigameName);
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
					if(levelManager.OnSelection)
						Camera.main.GetComponent<SmoothCameraMove>().Speed = Camera.main.GetComponent<SmoothCameraMove>().defaultSpeed/2;
					else
						Camera.main.GetComponent<SmoothCameraMove>().Speed = Camera.main.GetComponent<SmoothCameraMove>().defaultSpeed;
					Camera.main.GetComponent<SmoothCameraMove>().To = CameraZoom;
					Camera.main.GetComponent<SmoothCameraMove>().ToYRot = Camera.main.GetComponent<CameraControl>().currentWaypoint.transform.eulerAngles.y;
					Camera.main.GetComponent<CameraControl>().ReadyToLeave = false;
					Camera.main.GetComponent<CameraControl>().movingLeft = false;
					Camera.main.GetComponent<CameraControl>().movingRight = false;

					//deactivate OnSelection flag on previously zoomed minigame and set it to current minigame
					//if this is the first selected minigame, set it instantly
					if(levelManager.minigameOnSelection == null)
						levelManager.minigameOnSelection = this.gameObject;
					else
					{
						if(levelManager.minigameOnSelection != this.gameObject)
							levelManager.OnSelection = false;
						levelManager.minigameOnSelection = this.gameObject;
					}
					levelManager.OnSelection = true;
				}
			}
		}

		IEnumerator SmoothScaleUp()
		{
			float startTime = Time.time;
			StopCoroutine ("SmoothScaleDown");
			Vector3 startScale = Icon.transform.localScale;
			Vector3 targetScale = new Vector3(1, 1, 1);

			while(Icon.transform.localScale.x < 0.99f)
			{
				Icon.transform.localScale = Vector3.Lerp (startScale, targetScale, (Time.time - startTime) * 1.5f);
				yield return null;
			}
		}

		IEnumerator SmoothScaleDown()
		{
			float startTime = Time.time;
			StopCoroutine ("SmoothScaleUp");
			Vector3 startScale = Icon.transform.localScale;
			Vector3 targetScale = new Vector3(0.6f, 0.6f, 1);

			while(Icon.transform.localScale.x > 0.61f)
			{
				Icon.transform.localScale = Vector3.Lerp (startScale, targetScale, (Time.time - startTime) * 1.5f);
				yield return null;
			}
		}
	}
}