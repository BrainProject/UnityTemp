/*
 * Created by: Milan Doležal
 */ 


using UnityEngine;
using System.Collections;

public enum Side{none, Left, Right, Up, Down, Forward, Backward};

public class RotateAroundBrainBorder : MonoBehaviour {
	public Side rotatingDirection;
	public int speedOfRotation = 50;
	public bool CanRotate{ get; set; }
	private Vector3 rotatingAxis;
	
	// Use this for initialization
	void Start()
	{
		CanRotate = false;
		switch(rotatingDirection)
		{
		case Side.Left:
			rotatingAxis = Vector3.up;
			this.guiTexture.pixelInset = new Rect(0 - Screen.width/2, 0 - Screen.height/2, Screen.width/8, Screen.height);
			break;
		case Side.Right:
			rotatingAxis = Vector3.down;
			this.guiTexture.pixelInset = new Rect(Screen.width/2 - Screen.width/8, 0 - Screen.height/2, Screen.width/8, Screen.height);
			break;
		case Side.Forward:
			rotatingAxis = Vector3.forward;
			this.guiTexture.pixelInset = new Rect(0 - Screen.width/2, Screen.height/2 - 128, Screen.width, 128);
			break;
		case Side.Backward:
			rotatingAxis = Vector3.back;
			this.guiTexture.pixelInset = new Rect(0 - Screen.width/2, 0 - Screen.height/2, Screen.width, 128);
			break;
		case Side.Up:
			rotatingAxis = Vector3.left;
			this.guiTexture.pixelInset = new Rect(0 - Screen.width/2, Screen.height/2 - 128, Screen.width, 128);
			break;
		case Side.Down:
			rotatingAxis = Vector3.right;
			this.guiTexture.pixelInset = new Rect(0 - Screen.width/2, 0 - Screen.height/2, Screen.width, 128);
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver()
	{
		if(CanRotate)
		{
			if(rotatingDirection == Side.Left || rotatingDirection == Side.Right)
				//Camera.main.transform.RotateAround(this.transform.parent.transform.parent.position, rotatingAxis, Time.deltaTime * speedOfRotation * 1000);
				Camera.main.transform.parent.transform.Rotate(rotatingAxis * Time.deltaTime * speedOfRotation, Space.World);
			if(rotatingDirection == Side.Forward || rotatingDirection == Side.Up)
				if(!(Camera.main.transform.parent.transform.localEulerAngles.z > 50 && Camera.main.transform.parent.transform.localEulerAngles.z <= 300))
					Camera.main.transform.parent.transform.Rotate(rotatingAxis * Time.deltaTime * speedOfRotation, Space.Self);
			if(rotatingDirection == Side.Backward || rotatingDirection == Side.Down)
				if(!(Camera.main.transform.parent.transform.eulerAngles.z < 310 && Camera.main.transform.parent.transform.eulerAngles.z >= 60))
					Camera.main.transform.parent.transform.Rotate(rotatingAxis * Time.deltaTime * speedOfRotation, Space.Self);
		}
	}

//	void OnGUI()
//	{
//		if(CanRotate)
//			if(GUI.Button(new Rect(20, 20, 200, 40), "Reset rotation"))
//			   this.transform.parent.transform.parent.transform.eulerAngles = new Vector3(0,0,0);
//	}
}
