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
			System.Diagnostics.Process.Start("D:\\Sdílené\\Dropbox\\Dropbox\\Unity\\Project Serious Brain\\NonUnityAssests\\External\\Pexeso\\Pexeso.exe");
		}
	}
}
