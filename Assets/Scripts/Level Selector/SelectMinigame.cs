using UnityEngine;
using System.Collections;

public class SelectMinigame : MonoBehaviour {
	public string minigameName;
	//public bool clickable{ get; set; }

	private Transform cameraZoom;
	private bool OnSelection { get; set; }
	private Camera mainCamera { get; set; }
	// Use this for initialization
	void Start () {
		OnSelection = false;
		mainCamera = GameObject.Find ("Main Camera").camera;
		cameraZoom = transform.GetChild (0);
	}
	
	// Update is called once per frame
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
			print (cameraZoom.position);
			mainCamera.GetComponent<SmoothCameraMove>().To = cameraZoom.position;
			mainCamera.GetComponent<SmoothCameraMove>().Move = true;
			OnSelection = true;
		}
	}

	void OnGUI()
	{
		if(OnSelection)
			if(GUI.Button(new Rect(20,20,100,40), "Cancel"))
			{
				OnSelection = false;
				mainCamera.transform.position = new Vector3(0,1,-10);
				mainCamera.GetComponent<SmoothCameraMove>().From = new Vector3(0,1,-10);
				mainCamera.GetComponent<SmoothCameraMove>().To = new Vector3(0,1,-10);
			}
	}
}
