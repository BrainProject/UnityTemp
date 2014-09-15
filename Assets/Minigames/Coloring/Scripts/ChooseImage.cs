using UnityEngine;
using System.Collections;

public class ChooseImage : MonoBehaviour {

	public GameObject Image;
	public Animation DeskAnimation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown(){
		Image.SetActive(true);
		if(!DeskAnimation.IsPlaying("deskRotation") && !DeskAnimation.IsPlaying("deskRotation2"))
		{
			DeskAnimation.Play("deskRotation2");
		}
	}
}
