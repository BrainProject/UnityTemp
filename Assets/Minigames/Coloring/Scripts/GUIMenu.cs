using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIMenu : MonoBehaviour {

	public Animation DeskAnimation;
	public GameObject Images;

	void OnGUI (){	
		if(GUI.Button (new Rect(100,100,205,50),"Zpět")){
			if(!DeskAnimation.IsPlaying("deskRotation") && !DeskAnimation.IsPlaying("deskRotation2") && Images.activeSelf)
			{
				DeskAnimation.CrossFade("deskRotation");
			}else if(!Images.activeSelf)
			{
				Application.Quit();
			}
		}

	}
}
