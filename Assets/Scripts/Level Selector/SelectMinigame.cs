using UnityEngine;
using System.Collections;

public class SelectMinigame : MonoBehaviour {
	public string minigameName;
	//public bool clickable{ get; set; }

	private Vector3 CameraZoom { get; set; }
	private Vector3 CameraDefaultPosition { get; set; }
	private bool OnSelection { get; set; }
	private Camera mainCamera { get; set; }
	// Use this for initialization
	void Start () {
		OnSelection = false;
		CameraDefaultPosition = new Vector3(0,1,-10);
		mainCamera = GameObject.Find ("Main Camera").camera;
		CameraZoom = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z - 4);
	}

	void Update()
	{
		if(Input.GetButtonDown("Fire2"))
		{
			OnSelection = false;
			//mainCamera.transform.position = CameraDefaultPosition;
			mainCamera.GetComponent<SmoothCameraMove>().Move = true;
			mainCamera.GetComponent<SmoothCameraMove>().From = mainCamera.transform.position;
			mainCamera.GetComponent<SmoothCameraMove>().To = CameraDefaultPosition;
		}
	}

	
	void OnMouseEnter () {
		this.renderer.material.color = Color.green;
	}

	void OnMouseExit()
	{
		this.renderer.material.color = Color.white;
	}

	void OnMouseDown()
	{
		if(OnSelection)
			Application.LoadLevel (minigameName);
		else
		{
			//mainCamera.GetComponent<SmoothCameraMove>().from;
			//print (cameraZoom);
			mainCamera.GetComponent<SmoothCameraMove>().Move = true;
			mainCamera.GetComponent<SmoothCameraMove>().From = mainCamera.transform.position;
			mainCamera.GetComponent<SmoothCameraMove>().To = CameraZoom;
			OnSelection = true;
		}
	}
}
