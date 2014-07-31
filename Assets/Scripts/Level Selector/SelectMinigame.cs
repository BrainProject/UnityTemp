/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;
using Game;

namespace MinigameSelection {
	public class SelectMinigame : MonoBehaviour {
		public string minigameName;
		public float cameraDistance = 1;
		public string iconName;

		//Will be changed according to currently selected brain part.
		public Vector3 CameraDefaultPosition { get; set; }

		private bool MouseHover{ get; set; }
		private Vector3 CameraZoom { get; set; }
		private bool OnSelection { get; set; }
		private Camera mainCamera { get; set; }
		private GameObject Icon { get; set; }
		private Color OriginalColor { get; set; }

		void Start () {
			OnSelection = false;
			mainCamera = GameObject.Find ("Main Camera").camera;
			CameraDefaultPosition = GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition;
			CameraZoom = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z - cameraDistance);
			MouseHover = false;
			Icon = GameObject.Find ("Selection Part Icon");
			Icon.renderer.material.color = new Color(Icon.renderer.material.color.r, Icon.renderer.material.color.g, Icon.renderer.material.color.b, 0);
		}

		void Update()
		{
			if(Input.GetButtonDown("Fire1") && !MouseHover)
				OnSelection = false;
			//Set target position of camera back to its original point
			if(Input.GetButtonDown ("Vertical") || Input.GetMouseButtonDown(1))
			{
				OnSelection = false;
				//StartCoroutine(mainCamera.GetComponent<SmoothCameraMove>().CameraLerp(Time.time));
				mainCamera.GetComponent<SmoothCameraMove>().Move = true;
				mainCamera.GetComponent<SmoothCameraMove>().Speed = mainCamera.GetComponent<SmoothCameraMove>().defaultSpeed;
				mainCamera.GetComponent<SmoothCameraMove>().From = mainCamera.transform.position;
				mainCamera.GetComponent<SmoothCameraMove>().To = GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition;
			}
		}

		
		void OnMouseEnter()
		{
			OriginalColor = this.renderer.material.color;
			this.renderer.material.color = new Color(OriginalColor.r + 0.4f, OriginalColor.g + 0.4f, OriginalColor.b + 0.4f);
			Texture tmp = (Texture)Resources.Load ("Selection/" + iconName, typeof(Texture));
			//if(tmp != null)
			{
				Icon.renderer.material.mainTexture = tmp;
				Icon.renderer.material.color = new Color(Icon.renderer.material.color.r, Icon.renderer.material.color.g, Icon.renderer.material.color.b, 1);
				Icon.transform.position = this.transform.position;
			}
			//this.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
			MouseHover = true;

	        Logger.addLogEntry("Mouse enter the object: '" + this.name + "'");
		}

		void OnMouseExit()
		{
			this.renderer.material.color = OriginalColor;
			Icon.renderer.material.mainTexture = null;
			Icon.renderer.material.color = new Color(Icon.renderer.material.color.r, Icon.renderer.material.color.g, Icon.renderer.material.color.b, 0);
			MouseHover = false;

	        Logger.addLogEntry("Mouse exit the object: '" + this.name + "'");
		}

		void OnMouseOver()
		{
			if(Input.GetButtonDown("Fire1"))
			{
				//load minigame if zooming or zoomed
				if(OnSelection)
				{
					GameObject.Find ("LoadLevelWithFade").guiTexture.enabled = true;
					GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition = CameraZoom;
					StartCoroutine(GameObject.Find ("LoadLevelWithFade").GetComponent<LoadLevelWithFade>().LoadSeledctedLevelWithColorLerp(false, minigameName));
				}
				//set target position of camera near to minigame buble
				else
				{
					//StartCoroutine(mainCamera.GetComponent<SmoothCameraMove>().CameraLerp(Time.time));
					mainCamera.GetComponent<SmoothCameraMove>().Move = true;
					mainCamera.GetComponent<SmoothCameraMove>().From = mainCamera.transform.position;
					mainCamera.GetComponent<SmoothCameraMove>().Speed = mainCamera.GetComponent<SmoothCameraMove>().defaultSpeed;
					mainCamera.GetComponent<SmoothCameraMove>().To = CameraZoom;
					OnSelection = true;
				}
			}
		}
	}
}