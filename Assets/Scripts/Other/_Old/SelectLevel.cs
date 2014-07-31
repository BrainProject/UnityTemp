using UnityEngine;
using System.Collections;

public class SelectLevel : MonoBehaviour {

	private Color originalColor;

	void Start()
	{
		originalColor = renderer.material.color;
	}

	void OnMouseOver()
	{
		//if(Vector3.Distance(GameObject.Find("Player").transform.position,transform.position) < 5)
		renderer.material.color = Color.green;
	}

	void OnMouseExit()
	{
		renderer.material.color = originalColor;
	}

	void OnMouseDown()
	{
		//System.Diagnostics.Process.Start("notepad.exe");
		//System.Diagnostics.Process.Start ("D:\\Sdílené\\Dropbox\\Unity\\Project Serious Brain\\External Applications\\Pexeso\\Pexeso.exe");
	}
}