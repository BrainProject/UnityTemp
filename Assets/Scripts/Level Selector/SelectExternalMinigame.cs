using UnityEngine;
using System.Collections;

public class SelectExternalMinigame : MonoBehaviour {

	void OnMouseEnter () {
		this.renderer.material.color = Color.green;
	}
	
	void OnMouseExit()
	{
		this.renderer.material.color = Color.white;
	}

	void OnMouseOver()
	{
		if(Input.GetButtonDown ("Fire1"))
		{
			GameObject tmp = GameObject.Find("KinectControls");
			tmp.SetActive(false);
			tmp.GetComponent<KinectManager>().enabled = false;
			//need to consider parametrizable paths, kinect control (deactivate in Unity when external application starts and reactivate when it ends) and window focus return to Unity back with fullscreen
			System.Diagnostics.Process.Start("D:\\Sdílené\\Dropbox\\Dropbox\\Unity\\Project Serious Brain\\NonUnityAssests\\External\\Pexeso\\Pexeso.exe");
		}
	}
}
