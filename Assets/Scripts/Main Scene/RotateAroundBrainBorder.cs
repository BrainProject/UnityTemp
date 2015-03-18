/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;

namespace MainScene {
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
#if UNITY_ANDROID
			switch(rotatingDirection)
			{
			case Side.Left:
				rotatingAxis = Vector3.up;
				this.guiTexture.pixelInset = new Rect(-Screen.width/2, -Screen.height/2, Screen.width/4, Screen.height + 4);
				break;
			case Side.Right:
				rotatingAxis = Vector3.down;
				this.guiTexture.pixelInset = new Rect(Screen.width/2 - Screen.width/4, - Screen.height/2, Screen.width/4, Screen.height + 4);
				break;
			case Side.Forward:
				rotatingAxis = Vector3.forward;
				this.guiTexture.pixelInset = new Rect(-Screen.width/2, Screen.height/2 - Screen.height/8, Screen.width + 4, Screen.height / 8);
				break;
			case Side.Backward:
				rotatingAxis = Vector3.back;
				this.guiTexture.pixelInset = new Rect(-Screen.width/2, -Screen.height/2 - 1, Screen.width + 4, Screen.height / 8);
				break;
			case Side.Up:
				rotatingAxis = Vector3.left;
				this.guiTexture.pixelInset = new Rect(-Screen.width/2, Screen.height/2 - 128, Screen.width, Screen.height / 8);
				break;
			case Side.Down:
				rotatingAxis = Vector3.right;
				this.guiTexture.pixelInset = new Rect(-Screen.width/2, -Screen.height/2, Screen.width, Screen.height / 8);
				break;
			}
			this.guiTexture.color = new Color (this.guiTexture.color.r, this.guiTexture.color.g, this.guiTexture.color.b, 0);
#else
			switch(rotatingDirection)
			{
			case Side.Left:
				rotatingAxis = Vector3.up;
				this.guiTexture.pixelInset = new Rect(-Screen.width/2, -Screen.height/2, Screen.width/8, Screen.height + 4);
				break;
			case Side.Right:
				rotatingAxis = Vector3.down;
				this.guiTexture.pixelInset = new Rect(Screen.width/2 - Screen.width/8, - Screen.height/2, Screen.width/8, Screen.height + 4);
				break;
			case Side.Forward:
				rotatingAxis = Vector3.forward;
				this.guiTexture.pixelInset = new Rect(-Screen.width/2, Screen.height/2 - Screen.height/8, Screen.width + 4, Screen.height / 8);
				break;
			case Side.Backward:
				rotatingAxis = Vector3.back;
				this.guiTexture.pixelInset = new Rect(-Screen.width/2, -Screen.height/2 - 1, Screen.width + 4, Screen.height / 8);
				break;
			case Side.Up:
				rotatingAxis = Vector3.left;
				this.guiTexture.pixelInset = new Rect(-Screen.width/2, Screen.height/2 - 128, Screen.width, Screen.height / 8);
				break;
			case Side.Down:
				rotatingAxis = Vector3.right;
				this.guiTexture.pixelInset = new Rect(-Screen.width/2, -Screen.height/2, Screen.width, Screen.height / 8);
				break;
			}
			this.guiTexture.color = new Color (this.guiTexture.color.r, this.guiTexture.color.g, this.guiTexture.color.b, 0);
#endif
		}

#if UNITY_ANDROID
		void OnMouseDrag()
		{
			if(CanRotate)
			{
				this.guiTexture.color = new Color (this.guiTexture.color.r, this.guiTexture.color.g, this.guiTexture.color.b, 0.5f);
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

		void OnMouseUp()
		{
			if(CanRotate)
			{
				this.guiTexture.color = new Color (this.guiTexture.color.r, this.guiTexture.color.g, this.guiTexture.color.b, 0);
			}
		}
#else
		void OnMouseEnter()
		{
			if(CanRotate)
			{
				this.guiTexture.color = new Color (this.guiTexture.color.r, this.guiTexture.color.g, this.guiTexture.color.b, 0.5f);
			}
		}

		void OnMouseExit()
		{
			if(CanRotate)
			{
				this.guiTexture.color = new Color (this.guiTexture.color.r, this.guiTexture.color.g, this.guiTexture.color.b, 0);
			}
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
#endif

	//	void OnGUI()
	//	{
	//		if(CanRotate)
	//			if(GUI.Button(new Rect(20, 20, 200, 40), "Reset rotation"))
	//			   this.transform.parent.transform.parent.transform.eulerAngles = new Vector3(0,0,0);
	//	}
	}
}