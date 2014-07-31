/*
 * Created by: Milan Doležal
 */ 


using UnityEngine;
using System.Collections;

public class SelectMinigame : MonoBehaviour {
	public string minigameName;
	public float cameraDistance = 1;

	//Will be changed according to currently selected brain part.
	public Vector3 CameraDefaultPosition { get; set; }

	private bool MouseHover{ get; set; }
	private Vector3 CameraZoom { get; set; }
	private bool OnSelection { get; set; }
	private Camera mainCamera { get; set; }
	private Color originalColor;

	void Start () {
		OnSelection = false;
		mainCamera = GameObject.Find ("Main Camera").camera;
		CameraDefaultPosition = GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition;
		CameraZoom = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z - cameraDistance);
		MouseHover = false;
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
			mainCamera.GetComponent<SmoothCameraMove>().From = mainCamera.transform.position;
			mainCamera.GetComponent<SmoothCameraMove>().To = GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition;
		}
	}

	
	void OnMouseEnter()
	{
		originalColor = this.renderer.material.color;
		this.renderer.material.color = new Color(originalColor.r + 0.4f, originalColor.g + 0.4f, originalColor.b + 0.4f);
		MouseHover = true;

        Logger.addLogEntry("Mouse enter the object: '" + this.name + "'");
	}

	void OnMouseExit()
	{
		this.renderer.material.color = originalColor;
		MouseHover = false;
        Logger.addLogEntry("Mouse exit the object: '" + this.name + "'");
	}

	void OnMouseOver()
	{
		if(Input.GetButtonDown("Fire1") || Input.GetButtonDown ("Vertical"))
		{
			//load minigame if zooming or zoomed
			if(OnSelection)
			{
				GameObject.Find ("LoadLevelWithFade").guiTexture.enabled = true;
				StartCoroutine(GameObject.Find ("LoadLevelWithFade").GetComponent<LoadLevelWithFade>().LoadSeledctedLevelWithColorLerp(false, minigameName));
			}
			//set target position of camera near to minigame buble
			else
			{
				//StartCoroutine(mainCamera.GetComponent<SmoothCameraMove>().CameraLerp(Time.time));
				mainCamera.GetComponent<SmoothCameraMove>().Move = true;
				mainCamera.GetComponent<SmoothCameraMove>().From = mainCamera.transform.position;
				mainCamera.GetComponent<SmoothCameraMove>().To = CameraZoom;
				OnSelection = true;
			}
		}
	}
}
