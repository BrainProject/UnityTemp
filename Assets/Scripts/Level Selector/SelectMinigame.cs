using UnityEngine;
using System.Collections;

public class SelectMinigame : MonoBehaviour {
	public string minigameName;
	public float cameraDistance = 1;

	private bool MouseHover{ get; set; }
	private Vector3 CameraZoom { get; set; }
	private Vector3 CameraDefaultPosition { get; set; }
	private bool OnSelection { get; set; }
	private Camera mainCamera { get; set; }

	void Start () {
		OnSelection = false;
		mainCamera = GameObject.Find ("Main Camera").camera;
		CameraDefaultPosition = mainCamera.transform.position;
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
			//StartCoroutine(mainCamera.GetComponent<SmoothCameraMove>().CameraLerp(mainCamera.transform.position, CameraDefaultPosition));
			mainCamera.GetComponent<SmoothCameraMove>().Move = true;
			mainCamera.GetComponent<SmoothCameraMove>().From = mainCamera.transform.position;
			mainCamera.GetComponent<SmoothCameraMove>().To = CameraDefaultPosition;
		}
	}

	
	void OnMouseEnter () {
		this.renderer.material.color = Color.green;
		MouseHover = true;
	}

	void OnMouseExit()
	{
		this.renderer.material.color = Color.white;
		MouseHover = false;
	}

	void OnMouseOver()
	{
		if(Input.GetButtonDown("Fire1") || Input.GetButtonDown ("Vertical"))
		{
			//load minigame if zooming or zoomed
			if(OnSelection)
				Application.LoadLevel (minigameName);
			//set target position of camera near to minigame buble
			else
			{
				//StartCoroutine(mainCamera.GetComponent<SmoothCameraMove>().CameraLerp(mainCamera.transform.position, CameraZoom));
				mainCamera.GetComponent<SmoothCameraMove>().Move = true;
				mainCamera.GetComponent<SmoothCameraMove>().From = mainCamera.transform.position;
				mainCamera.GetComponent<SmoothCameraMove>().To = CameraZoom;
				OnSelection = true;
			}
		}
	}
}
